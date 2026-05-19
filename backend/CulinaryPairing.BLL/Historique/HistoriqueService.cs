using CulinaryPairing.BLL.DTOs;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Historique;

public class HistoriqueService : IHistoriqueService
{
    /// <summary>
    /// Fenêtre de déduplication : si l'utilisateur consulte la même recette
    /// dans cet intervalle, l'événement n'est PAS ré-enregistré.
    /// Évite que F5 répétés polluent le widget dashboard.
    /// </summary>
    private const int DedupWindowMinutes = 5;

    private readonly CulinaryPairingDbContext _context;

    public HistoriqueService(CulinaryPairingDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AjouterAsync(int idUtilisateur, int idRecette, CancellationToken ct = default)
    {
        var seuilDedup = DateTime.UtcNow.AddMinutes(-DedupWindowMinutes);

        var consultationRecente = await _context.HistoriquesConsultations
            .AnyAsync(
                h => h.IdUtilisateur == idUtilisateur
                  && h.IdRecette == idRecette
                  && h.DateConsultation >= seuilDedup,
                ct);

        if (consultationRecente)
        {
            return false;
        }

        _context.HistoriquesConsultations.Add(new HistoriqueConsultation
        {
            IdUtilisateur = idUtilisateur,
            IdRecette = idRecette,
            DateConsultation = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IReadOnlyList<HistoriqueRecetteDto>> GetDernieresRecettesAsync(
        int idUtilisateur,
        int limit = 5,
        CancellationToken ct = default)
    {
        if (limit < 1) limit = 1;
        if (limit > 20) limit = 20;

        // Dernière consultation par recette pour cet utilisateur,
        // jointe à la recette pour récupérer titre / image / type.
        var dernieres = await _context.HistoriquesConsultations
            .Where(h => h.IdUtilisateur == idUtilisateur)
            .GroupBy(h => h.IdRecette)
            .Select(g => new
            {
                IdRecette = g.Key,
                DerniereConsultation = g.Max(h => h.DateConsultation)
            })
            .OrderByDescending(x => x.DerniereConsultation)
            .Take(limit)
            .Join(
                _context.Recettes,
                x => x.IdRecette,
                r => r.IdRecette,
                (x, r) => new HistoriqueRecetteDto
                {
                    IdRecette = r.IdRecette,
                    Titre = r.Titre,
                    ImageUrl = r.ImageUrl,
                    TypePlat = r.TypePlat.ToString() ?? string.Empty,
                    DerniereConsultation = x.DerniereConsultation
                })
            .OrderByDescending(d => d.DerniereConsultation)
            .ToListAsync(ct);

        return dernieres;
    }
}