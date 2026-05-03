using CulinaryPairing.BLL.DTOs.Soirees;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Soirees;

public class MenusService : IMenusService
{
    private readonly CulinaryPairingDbContext _db;
    private readonly ISoireesService _soirees;

    public MenusService(CulinaryPairingDbContext db, ISoireesService soirees)
    {
        _db = db;
        _soirees = soirees;
    }

    // ---------- GET-OR-CREATE ----------
    public async Task<MenuDto?> GetOrCreateAsync(int idSoiree, int idUtilisateur)
    {
        if (!await SoireeAppartientAuUserAsync(idSoiree, idUtilisateur)) return null;

        var menu = await ChargerMenuAvecRecettes(idSoiree);
        if (menu == null)
        {
            menu = new MenuSoiree { IdSoiree = idSoiree };
            _db.MenusSoiree.Add(menu);
            await _db.SaveChangesAsync();
            // Pas de recettes encore → recharge inutile, on construit le DTO vide directement
            return new MenuDto { IdMenu = menu.IdMenu };
        }

        return MapToDto(menu);
    }

    // ---------- ASSIGN ----------
    public async Task<MenuDto?> AssignSlotAsync(int idSoiree, string slot, int idRecette, int idUtilisateur)
    {
        if (!await SoireeAppartientAuUserAsync(idSoiree, idUtilisateur)) return null;

        var typePlatAttendu = ParseSlot(slot); // throws ArgumentException si invalide

        // Vérif recette : existe + publiée + type_plat match
        var recette = await _db.Recettes
            .Where(r => r.IdRecette == idRecette && r.Statut == StatutRecette.Publiee)
            .Select(r => new { r.IdRecette, r.TypePlat })
            .FirstOrDefaultAsync();

        if (recette == null)
            throw new ArgumentException($"Recette {idRecette} introuvable ou non publiée.");

        if (recette.TypePlat != typePlatAttendu)
            throw new ArgumentException(
                $"La recette {idRecette} a type_plat={recette.TypePlat} et ne peut être assignée au slot {typePlatAttendu}.");

        // Vérif contraintes agrégées (cycle 7 logic anticipée ici en garde API)
        await ValiderContraintesViolees(idSoiree, idUtilisateur, idRecette);

        // Assigne sur le menu (get-or-create silencieux côté entité)
        var menu = await _db.MenusSoiree.FirstOrDefaultAsync(m => m.IdSoiree == idSoiree)
                   ?? AjouterMenuVide(idSoiree);

        AffecterSlot(menu, typePlatAttendu, idRecette);
        await RecalculerTotaux(menu);

        await _db.SaveChangesAsync();

        var menuComplet = await ChargerMenuAvecRecettes(idSoiree);
        return MapToDto(menuComplet!);
    }

    // ---------- UNASSIGN ----------
    public async Task<MenuDto?> UnassignSlotAsync(int idSoiree, string slot, int idUtilisateur)
    {
        if (!await SoireeAppartientAuUserAsync(idSoiree, idUtilisateur)) return null;

        var typePlat = ParseSlot(slot);

        var menu = await _db.MenusSoiree.FirstOrDefaultAsync(m => m.IdSoiree == idSoiree);
        if (menu == null) return new MenuDto(); // rien à désassigner

        AffecterSlot(menu, typePlat, null);
        await RecalculerTotaux(menu);

        await _db.SaveChangesAsync();

        var menuComplet = await ChargerMenuAvecRecettes(idSoiree);
        return MapToDto(menuComplet!);
    }

    // ---------- RECETTES ÉLIGIBLES ----------
    public async Task<List<RecetteSlotDto>?> GetRecettesEligiblesAsync(int idSoiree, string slot, int idUtilisateur)
    {
        if (!await SoireeAppartientAuUserAsync(idSoiree, idUtilisateur)) return null;

        var typePlat = ParseSlot(slot); // throws si invalide

        var contraintesAgregees = await _soirees.GetContraintesAgregeesAsync(idSoiree, idUtilisateur);
        var idsContraintesAgregees = contraintesAgregees.Select(c => c.IdContrainte).ToHashSet();

        // Base : recettes publiées du bon type
        var query = _db.Recettes
            .AsNoTracking()
            .Where(r => r.Statut == StatutRecette.Publiee && r.TypePlat == typePlat);

        // Filtre contraintes : si aucune agrégée, toutes les recettes du type passent
        if (idsContraintesAgregees.Count > 0)
        {
            // Une recette est éligible si AUCUN de ses ingrédients ne porte une contrainte interdite.
            // Traduit en SQL : NOT EXISTS (ingredients dont une contrainte est dans idsContraintesAgregees).
            query = query.Where(r =>
                !r.Ingredients.Any(ri =>
                    ri.Ingredient.Contraintes.Any(ic =>
                        idsContraintesAgregees.Contains(ic.IdContrainte))));
        }

        return await query
            .OrderBy(r => r.Titre)
            .Select(r => new RecetteSlotDto
            {
                IdRecette = r.IdRecette,
                Titre = r.Titre,
                ImageUrl = r.ImageUrl,
                TypePlat = r.TypePlat!.Value.ToString(),
                TempsPreparation = r.TempsPreparation,
                TempsCuisson = r.TempsCuisson,
                CoutEstime = r.CoutEstime
            })
            .ToListAsync();
    }
    
    // ========== HELPERS ==========

    private async Task<bool> SoireeAppartientAuUserAsync(int idSoiree, int idUtilisateur)
        => await _db.Soirees.AnyAsync(s => s.IdSoiree == idSoiree && s.IdUtilisateur == idUtilisateur);

    private static TypePlat ParseSlot(string slot) => slot.ToLowerInvariant() switch
    {
        "entree" => TypePlat.Entree,
        "plat" => TypePlat.Plat,
        "dessert" => TypePlat.Dessert,
        _ => throw new ArgumentException($"Slot invalide : '{slot}'. Attendu : entree|plat|dessert.")
    };

    private async Task ValiderContraintesViolees(int idSoiree, int idUtilisateur, int idRecette)
    {
        var contraintesAgregees = await _soirees.GetContraintesAgregeesAsync(idSoiree, idUtilisateur);
        if (contraintesAgregees.Count == 0) return;

        var idsContraintesAgregees = contraintesAgregees.Select(c => c.IdContrainte).ToHashSet();

        // Une recette VIOLE une contrainte si au moins un de ses ingrédients porte cette contrainte.
        var contraintesViolees = await _db.RecettesIngredients
            .Where(ri => ri.IdRecette == idRecette)
            .SelectMany(ri => ri.Ingredient.Contraintes)
            .Where(ic => idsContraintesAgregees.Contains(ic.IdContrainte))
            .Select(ic => ic.Contrainte.Nom)
            .Distinct()
            .ToListAsync();

        if (contraintesViolees.Count > 0)
            throw new ArgumentException(
                $"La recette {idRecette} viole les contraintes agrégées : {string.Join(", ", contraintesViolees)}.");
    }

    private MenuSoiree AjouterMenuVide(int idSoiree)
    {
        var menu = new MenuSoiree { IdSoiree = idSoiree };
        _db.MenusSoiree.Add(menu);
        return menu;
    }

    private static void AffecterSlot(MenuSoiree menu, TypePlat slot, int? idRecette)
    {
        switch (slot)
        {
            case TypePlat.Entree:  menu.IdRecetteEntree = idRecette; break;
            case TypePlat.Plat:    menu.IdRecettePlat = idRecette; break;
            case TypePlat.Dessert: menu.IdRecetteDessert = idRecette; break;
        }
    }

    private async Task RecalculerTotaux(MenuSoiree menu)
    {
        var ids = new[] { menu.IdRecetteEntree, menu.IdRecettePlat, menu.IdRecetteDessert }
            .Where(id => id.HasValue).Select(id => id!.Value).ToList();

        if (ids.Count == 0)
        {
            menu.CoutTotalEstime = null;
            menu.TempsTotalEstime = null;
            return;
        }

        var recettes = await _db.Recettes
            .AsNoTracking()
            .Where(r => ids.Contains(r.IdRecette))
            .Select(r => new { r.CoutEstime, r.TempsPreparation, r.TempsCuisson })
            .ToListAsync();

        // Coût : null si toutes nulles, sinon somme des non-nulles (choix : un coût manquant ne bloque pas le total)
        menu.CoutTotalEstime = recettes.Any(r => r.CoutEstime.HasValue)
            ? recettes.Where(r => r.CoutEstime.HasValue).Sum(r => r.CoutEstime!.Value)
            : null;

        // Temps : prep + cuisson de chaque recette, addition simple
        // Limitation MVP : ne modélise pas les chevauchements (préparable_avance, parallélisme four).
        menu.TempsTotalEstime = recettes.Sum(r => (r.TempsPreparation ?? 0) + (r.TempsCuisson ?? 0));
        if (menu.TempsTotalEstime == 0) menu.TempsTotalEstime = null;
    }

    private async Task<MenuSoiree?> ChargerMenuAvecRecettes(int idSoiree)
        => await _db.MenusSoiree
            .AsNoTracking()
            .Include(m => m.RecetteEntree)
            .Include(m => m.RecettePlat)
            .Include(m => m.RecetteDessert)
            .FirstOrDefaultAsync(m => m.IdSoiree == idSoiree);

    private static MenuDto MapToDto(MenuSoiree m) => new()
    {
        IdMenu = m.IdMenu,
        Entree = MapRecette(m.RecetteEntree),
        Plat = MapRecette(m.RecettePlat),
        Dessert = MapRecette(m.RecetteDessert),
        CoutTotalEstime = m.CoutTotalEstime,
        TempsTotalEstime = m.TempsTotalEstime
    };

    private static RecetteSlotDto? MapRecette(Recette? r) => r == null ? null : new RecetteSlotDto
    {
        IdRecette = r.IdRecette,
        Titre = r.Titre,
        ImageUrl = r.ImageUrl,
        TypePlat = r.TypePlat?.ToString(),
        TempsPreparation = r.TempsPreparation,
        TempsCuisson = r.TempsCuisson,
        CoutEstime = r.CoutEstime
    };
}