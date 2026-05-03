using CulinaryPairing.BLL.Contraintes;
using CulinaryPairing.BLL.DTOs.Soirees;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Soirees;

public class SoireesService : ISoireesService
{
    // Couplage assumé avec ContraintesSeed.cs (V1.3, 8 contraintes).
    // À améliorer : injecter via configuration ou résoudre au démarrage.
    private const string NomContrainteVegan = "Végan";
    private const string NomContrainteVegetarien = "Végétarien";
    private readonly CulinaryPairingDbContext _db;

    public SoireesService(CulinaryPairingDbContext db) => _db = db;

    // ---------- LECTURE LISTE ----------
    public async Task<List<SoireeListItemDto>> GetMineAsync(int idUtilisateur)
    {
        // AsNoTracking : lecture pure, pas de modification → perf + cohérence pattern projet.
        return await _db.Soirees
            .AsNoTracking()
            .Where(s => s.IdUtilisateur == idUtilisateur)
            .OrderByDescending(s => s.DateCreation)
            .Select(s => new SoireeListItemDto
            {
                IdSoiree = s.IdSoiree,
                NombrePersonnes = s.NombrePersonnes ?? 0,
                TypeSoiree = s.TypeSoiree.HasValue ? s.TypeSoiree.Value.ToString() : null,
                DateCreation = s.DateCreation,
                NbContraintes = s.Contraintes.Count,
                MenuComplet = s.Menus.Any(m =>
                    m.IdRecetteEntree != null &&
                    m.IdRecettePlat != null &&
                    m.IdRecetteDessert != null)
            })
            .ToListAsync();
    }

    // ---------- LECTURE DÉTAIL ----------
    public async Task<SoireeDetailDto?> GetByIdAsync(int idSoiree, int idUtilisateur)
    {
        var s = await _db.Soirees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.IdSoiree == idSoiree && x.IdUtilisateur == idUtilisateur);

        if (s == null) return null;

        var contraintesAgregees = await GetContraintesAgregeesAsync(idSoiree, idUtilisateur);

        return new SoireeDetailDto
        {
            IdSoiree = s.IdSoiree,
            NombrePersonnes = s.NombrePersonnes ?? 0,
            NombreVegetariens = s.NombreVegetariens,
            NombreVegans = s.NombreVegans,
            Budget = s.Budget,
            TempsDisponible = s.TempsDisponible,
            TypeSoiree = s.TypeSoiree?.ToString(),
            PreferenceAlcool = s.PreferenceAlcool.ToString(),
            DateCreation = s.DateCreation,
            ContraintesAgregees = contraintesAgregees,
            Menu = null // cycle 6
        };
    }

    // ---------- CREATE ----------
    public async Task<int> CreateAsync(int idUtilisateur, SoireeCreateDto dto)
    {
        ValiderCohortes(dto.NombrePersonnes, dto.NombreVegetariens, dto.NombreVegans);
        await ValiderContraintesIdsAsync(dto.ContraintesIds);

        var soiree = new Soiree
        {
            IdUtilisateur = idUtilisateur,
            NombrePersonnes = dto.NombrePersonnes,
            NombreVegetariens = dto.NombreVegetariens,
            NombreVegans = dto.NombreVegans,
            Budget = dto.Budget,
            TempsDisponible = dto.TempsDisponible,
            TypeSoiree = dto.TypeSoiree,
            PreferenceAlcool = dto.PreferenceAlcool,
            DateCreation = DateTime.UtcNow
        };

        foreach (var idContrainte in dto.ContraintesIds.Distinct())
        {
            soiree.Contraintes.Add(new SoireeContrainte { IdContrainte = idContrainte });
        }

        _db.Soirees.Add(soiree);
        await _db.SaveChangesAsync();
        return soiree.IdSoiree;
    }

    // ---------- UPDATE ----------
    public async Task<bool> UpdateAsync(int idSoiree, int idUtilisateur, SoireeUpdateDto dto)
    {
        ValiderCohortes(dto.NombrePersonnes, dto.NombreVegetariens, dto.NombreVegans);
        await ValiderContraintesIdsAsync(dto.ContraintesIds);

        var soiree = await _db.Soirees
            .Include(s => s.Contraintes)
            .FirstOrDefaultAsync(s => s.IdSoiree == idSoiree && s.IdUtilisateur == idUtilisateur);

        if (soiree == null) return false;

        soiree.NombrePersonnes = dto.NombrePersonnes;
        soiree.NombreVegetariens = dto.NombreVegetariens;
        soiree.NombreVegans = dto.NombreVegans;
        soiree.Budget = dto.Budget;
        soiree.TempsDisponible = dto.TempsDisponible;
        soiree.TypeSoiree = dto.TypeSoiree;
        soiree.PreferenceAlcool = dto.PreferenceAlcool;

        // Pattern delete + insert (cohérent avec UpdateUserContraintesAsync, idempotent, transactionnel via SaveChanges)
        _db.SoireesContraintes.RemoveRange(soiree.Contraintes);
        foreach (var idContrainte in dto.ContraintesIds.Distinct())
        {
            _db.SoireesContraintes.Add(new SoireeContrainte
            {
                IdSoiree = idSoiree,
                IdContrainte = idContrainte
            });
        }

        await _db.SaveChangesAsync();
        return true;
    }

    // ---------- DELETE ----------
    public async Task<bool> DeleteAsync(int idSoiree, int idUtilisateur)
    {
        var soiree = await _db.Soirees
            .FirstOrDefaultAsync(s => s.IdSoiree == idSoiree && s.IdUtilisateur == idUtilisateur);

        if (soiree == null) return false;

        // Cascade configurée DbContext : SOIREE_CONTRAINTE et MENU_SOIREE seront supprimés automatiquement.
        _db.Soirees.Remove(soiree);
        await _db.SaveChangesAsync();
        return true;
    }
    // ---------- AGRÉGATION CONTRAINTES (cycle 4) ----------
    public async Task<List<ContrainteDto>> GetContraintesAgregeesAsync(int idSoiree, int idUtilisateur)
    {
        var soiree = await _db.Soirees
            .AsNoTracking()
            .Include(s => s.Contraintes)
                .ThenInclude(sc => sc.Contrainte)
            .FirstOrDefaultAsync(s => s.IdSoiree == idSoiree && s.IdUtilisateur == idUtilisateur);

        if (soiree == null) return new List<ContrainteDto>();

        // Démarre avec les contraintes saisies par l'utilisateur (dédupli par id).
        var resultat = soiree.Contraintes
            .Select(sc => new ContrainteDto
            {
                IdContrainte = sc.Contrainte.IdContrainte,
                Nom = sc.Contrainte.Nom,
                Type = sc.Contrainte.Type.ToString()
            })
            .ToDictionary(c => c.IdContrainte, c => c);

        // Règle d'agrégation : Végan englobe Végétarien (sur-ensemble strict).
        // Un seul invité végan suffit à imposer la contrainte végan pour tout le menu.
        string? nomAjouter = null;
        if (soiree.NombreVegans > 0) nomAjouter = NomContrainteVegan;
        else if (soiree.NombreVegetariens > 0) nomAjouter = NomContrainteVegetarien;

        if (nomAjouter != null)
        {
            var contrainte = await _db.ContraintesAlimentaires
                .AsNoTracking()
                .Where(c => c.Nom == nomAjouter)
                .Select(c => new ContrainteDto
                {
                    IdContrainte = c.IdContrainte,
                    Nom = c.Nom,
                    Type = c.Type.ToString()
                })
                .FirstOrDefaultAsync();

            if (contrainte != null && !resultat.ContainsKey(contrainte.IdContrainte))
                resultat[contrainte.IdContrainte] = contrainte;
        }

        return resultat.Values.OrderBy(c => c.Type).ThenBy(c => c.Nom).ToList();
    }

    // ========== VALIDATIONS PRIVÉES ==========
    private static void ValiderCohortes(int nombrePersonnes, int vegetariens, int vegans)
    {
        // Défense en profondeur : le CHECK SQL est le dernier rempart, on attrape ici pour 400 propre.
        if (vegetariens + vegans > nombrePersonnes)
            throw new ArgumentException(
                $"Le total végétariens ({vegetariens}) + vegans ({vegans}) ne peut pas dépasser le nombre de personnes ({nombrePersonnes}).");
    }

    private async Task ValiderContraintesIdsAsync(List<int> ids)
    {
        if (ids.Count == 0) return;
        var idsValides = await _db.ContraintesAlimentaires
            .Where(c => ids.Contains(c.IdContrainte))
            .Select(c => c.IdContrainte)
            .ToListAsync();

        var invalides = ids.Distinct().Except(idsValides).ToList();
        if (invalides.Any())
            throw new ArgumentException($"Contraintes inexistantes : {string.Join(", ", invalides)}");
    }
}