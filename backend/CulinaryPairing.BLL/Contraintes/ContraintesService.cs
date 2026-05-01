using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Contraintes;

public class ContraintesService : IContraintesService
{
    private readonly CulinaryPairingDbContext _db;

    public ContraintesService(CulinaryPairingDbContext db) => _db = db;

    public async Task<List<ContrainteDto>> GetAllAsync()
    {
        // Matérialisation puis projection (pattern FavorisService) : évite traduction LINQ→SQL des enum HasConversion<string>
        var contraintes = await _db.ContraintesAlimentaires
            .OrderBy(c => c.Nom)
            .ToListAsync();

        return contraintes.Select(c => new ContrainteDto
        {
            IdContrainte = c.IdContrainte,
            Nom = c.Nom,
            Type = c.Type.ToString()
        }).ToList();
    }

    public async Task<List<ContrainteDto>> GetByUserAsync(int idUtilisateur)
    {
        var liaisons = await _db.UtilisateursContraintes
            .Include(uc => uc.Contrainte)
            .Where(uc => uc.IdUtilisateur == idUtilisateur)
            .ToListAsync();

        return liaisons
            .OrderBy(uc => uc.Contrainte.Nom)
            .Select(uc => new ContrainteDto
            {
                IdContrainte = uc.Contrainte.IdContrainte,
                Nom = uc.Contrainte.Nom,
                Type = uc.Contrainte.Type.ToString()
            }).ToList();
    }

    public async Task UpdateUserContraintesAsync(int idUtilisateur, List<int> idsContraintes)
    {
        // Validation : tous les ids fournis doivent exister dans le catalogue
        var distinct = idsContraintes.Distinct().ToList();
        if (distinct.Count > 0)
        {
            var existants = await _db.ContraintesAlimentaires
                .Where(c => distinct.Contains(c.IdContrainte))
                .Select(c => c.IdContrainte)
                .ToListAsync();
            var invalides = distinct.Except(existants).ToList();
            if (invalides.Any())
                throw new ArgumentException(
                    $"Contrainte(s) inexistante(s) : {string.Join(", ", invalides)}");
        }

        // Stratégie simple : delete + insert (liste courte, max ~8 entrées)
        var existingLinks = await _db.UtilisateursContraintes
            .Where(uc => uc.IdUtilisateur == idUtilisateur)
            .ToListAsync();
        _db.UtilisateursContraintes.RemoveRange(existingLinks);

        foreach (var id in distinct)
        {
            _db.UtilisateursContraintes.Add(new UtilisateurContrainte
            {
                IdUtilisateur = idUtilisateur,
                IdContrainte = id
            });
        }

        await _db.SaveChangesAsync();
    }
    
    public async Task<Dictionary<int, List<ContrainteDto>>> GetContraintesVioleesAsync(
        int idUtilisateur,
        List<int> idsRecettes)
    {
        if (idsRecettes.Count == 0)
            return new Dictionary<int, List<ContrainteDto>>();

        // 1. Récupérer les ids contraintes activées par le user
        var userContraintesIds = await _db.UtilisateursContraintes
            .Where(uc => uc.IdUtilisateur == idUtilisateur)
            .Select(uc => uc.IdContrainte)
            .ToListAsync();

        if (userContraintesIds.Count == 0)
            return new Dictionary<int, List<ContrainteDto>>();

        // 2. Joindre RECETTE_INGREDIENT → INGREDIENT_CONTRAINTE → CONTRAINTE_ALIMENTAIRE
        //    Filtrer sur les recettes demandées + contraintes du user
        var raw = await (
            from ri in _db.RecettesIngredients
            where idsRecettes.Contains(ri.IdRecette)
            join ic in _db.IngredientsContraintes on ri.IdIngredient equals ic.IdIngredient
            where userContraintesIds.Contains(ic.IdContrainte)
            join c in _db.ContraintesAlimentaires on ic.IdContrainte equals c.IdContrainte
            select new { ri.IdRecette, c.IdContrainte, c.Nom, c.Type }
        ).Distinct().ToListAsync();

        // 3. Grouper par recette
        return raw
            .GroupBy(x => x.IdRecette)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => new ContrainteDto
                {
                    IdContrainte = x.IdContrainte,
                    Nom = x.Nom,
                    Type = x.Type.ToString()
                }).ToList()
            );
    }
}