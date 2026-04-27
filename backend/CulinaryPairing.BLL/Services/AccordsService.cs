using CulinaryPairing.BLL.DTOs.Accords;
using CulinaryPairing.BLL.PairingEngine;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Services;

/// <summary>
/// Service d'accords. Stratégie cache-then-compute (CdC v1.3 §2.5) :
///   1. Pour chaque boisson, on cherche un ACCORD en cache à la version moteur courante.
///   2. Cache miss → calcul via PairingEngineService, puis persistance pour réutilisation.
///   3. Tri par score décroissant.
/// L'invalidation du cache repose sur le couple (UNIQUE recette/boisson/type, version_moteur) :
/// quand le moteur passe en V2.0, les accords V1.3 sont automatiquement obsolètes
/// et seront recalculés au prochain accès.
/// </summary>
public class AccordsService : IAccordsService
{
    private readonly CulinaryPairingDbContext _context;
    private readonly IPairingEngineService _engine;
    private const string VersionMoteurCourante = "1.3";

    public AccordsService(
        CulinaryPairingDbContext context,
        IPairingEngineService engine)
    {
        _context = context;
        _engine = engine;
    }

    public async Task<List<AccordDto>> GetAccordsByRecetteAsync(int idRecette)
    {
        // Recette + familles aromatiques (R19bis, R25bis)
        var recette = await _context.Recettes
            .Include(r => r.FamillesAromatiques)
                .ThenInclude(rf => rf.Famille)
            .FirstOrDefaultAsync(r => r.IdRecette == idRecette);

        if (recette is null)
            return new List<AccordDto>();

        // Toutes les boissons + leurs familles aromatiques
        var boissons = await _context.Boissons
            .Include(b => b.FamillesAromatiques)
                .ThenInclude(bf => bf.Famille)
            .ToListAsync();

        // Cache existant pour cette recette à la version courante
        var cacheExistant = await _context.Accords
            .Where(a => a.IdRecette == idRecette
                     && a.TypeAccord == TypeAccord.Regle
                     && a.VersionMoteur == VersionMoteurCourante)
            .ToDictionaryAsync(a => a.IdBoisson);

        var accords = new List<Accord>();

        foreach (var boisson in boissons)
        {
            if (cacheExistant.TryGetValue(boisson.IdBoisson, out var enCache))
            {
                accords.Add(enCache);
                continue;
            }

            // Cache miss → calcul + persistance
            var resultat = _engine.CalculerScore(recette, boisson);

            var nouvelAccord = new Accord
            {
                IdRecette = idRecette,
                IdBoisson = boisson.IdBoisson,
                TypeAccord = TypeAccord.Regle,
                ScoreCompatibilite = resultat.Score,
                NiveauConfiance = resultat.Confiance,
                MalusApplique = resultat.MalusApplique,
                ReglesSatisfaites = string.Join(",", resultat.ReglesSatisfaites),
                Justification = resultat.Justification,
                DateCalcul = DateTime.UtcNow,
                VersionMoteur = resultat.VersionMoteur
            };

            _context.Accords.Add(nouvelAccord);
            accords.Add(nouvelAccord);
        }

        if (accords.Any(a => _context.Entry(a).State == EntityState.Added))
            await _context.SaveChangesAsync();

        // Projection en DTO + tri par score décroissant
        return accords
            .OrderByDescending(a => a.ScoreCompatibilite)
            .Select(a => new AccordDto
            {
                IdAccord = a.IdAccord,
                TypeAccord = a.TypeAccord?.ToString(),
                Justification = a.Justification,
                ScoreCompatibilite = a.ScoreCompatibilite,
                NiveauConfiance = a.NiveauConfiance,
                MalusApplique = a.MalusApplique,
                ReglesSatisfaites = a.ReglesSatisfaites,
                DateCalcul = a.DateCalcul,
                VersionMoteur = a.VersionMoteur,
                IdBoisson = a.IdBoisson,
                NomBoisson = boissons.First(b => b.IdBoisson == a.IdBoisson).Nom,
                TypeBoisson = boissons.First(b => b.IdBoisson == a.IdBoisson).TypeBoisson?.ToString()
            })
            .ToList();
    }
}