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
    private const string VersionMoteurCourante = "1.3.1";

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
    public async Task<List<AccordInverseDto>?> GetRecettesByBoissonAsync(int idBoisson, int limit = 20)
    {
        // Clamp défensif côté service (le controller validera aussi en amont)
        if (limit < 1) limit = 1;
        if (limit > 50) limit = 50;

        // Boisson + familles aromatiques (R19bis, R25bis)
        var boisson = await _context.Boissons
            .Include(b => b.FamillesAromatiques)
                .ThenInclude(bf => bf.Famille)
            .FirstOrDefaultAsync(b => b.IdBoisson == idBoisson);

        // Distinction "boisson inconnue" vs "boisson sans accord" → null = 404
        if (boisson is null)
            return null;

        // Recettes publiées uniquement + familles aromatiques (miroir du sens direct)
        var recettes = await _context.Recettes
            .Include(r => r.FamillesAromatiques)
                .ThenInclude(rf => rf.Famille)
            .Where(r => r.Statut == StatutRecette.Publiee)
            .ToListAsync();

        if (recettes.Count == 0)
            return new List<AccordInverseDto>();

        // Cache existant pour cette boisson à la version courante (réutilise l'index unique
        // bidirectionnel UX_Accord_Recette_Boisson_Type)
        var cacheExistant = await _context.Accords
            .Where(a => a.IdBoisson == idBoisson
                     && a.TypeAccord == TypeAccord.Regle
                     && a.VersionMoteur == VersionMoteurCourante)
            .ToDictionaryAsync(a => a.IdRecette);

        var accords = new List<Accord>();

        foreach (var recette in recettes)
        {
            if (cacheExistant.TryGetValue(recette.IdRecette, out var enCache))
            {
                accords.Add(enCache);
                continue;
            }

            // Cache miss → calcul via le moteur figé v1.3, puis persistance pour réutilisation
            // bidirectionnelle (un appel /recettes/{id}/accords ultérieur réutilisera ce cache)
            var resultat = _engine.CalculerScore(recette, boisson);

            var nouvelAccord = new Accord
            {
                IdRecette = recette.IdRecette,
                IdBoisson = idBoisson,
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

        // Tri DESC par score, puis par confiance pour départager, puis limit
        return accords
            .OrderByDescending(a => a.ScoreCompatibilite)
            .ThenByDescending(a => a.NiveauConfiance)
            .Take(limit)
            .Select(a =>
            {
                var r = recettes.First(x => x.IdRecette == a.IdRecette);
                return new AccordInverseDto
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
                    IdRecette = r.IdRecette,
                    Titre = r.Titre,
                    ImageUrl = r.ImageUrl,
                    TypePlat = r.TypePlat?.ToString(),
                    Difficulte = r.Difficulte?.ToString(),
                    TempsPreparation = r.TempsPreparation,
                    TempsCuisson = r.TempsCuisson
                };
            })
            .ToList();
    }
}