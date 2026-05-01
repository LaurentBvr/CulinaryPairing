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
}