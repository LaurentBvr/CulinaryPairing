using CulinaryPairing.BLL.DTOs;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Services;

public class SearchService : ISearchService
{
    private readonly CulinaryPairingDbContext _context;

    public SearchService(CulinaryPairingDbContext context)
    {
        _context = context;
    }

    public async Task<SearchResultDto> SearchAsync(string query, int limitPerCategory = 5)
    {
        var result = new SearchResultDto();

        // Garde-fou : requête trop courte → résultat vide (évite de scanner toute la BDD sur 1 char)
        if (string.IsNullOrWhiteSpace(query) || query.Trim().Length < 2)
            return result;

        // Clamp défensif (controller validera aussi)
        if (limitPerCategory < 1) limitPerCategory = 1;
        if (limitPerCategory > 50) limitPerCategory = 50;

        var q = query.Trim();

        // --- Recettes publiées uniquement (titre)
        result.Recettes = await _context.Recettes
            .Where(r => r.Statut == StatutRecette.Publiee && r.Titre.Contains(q))
            .OrderBy(r => r.Titre)
            .Take(limitPerCategory)
            .Select(r => new SearchRecetteItemDto
            {
                Id = r.IdRecette,
                Titre = r.Titre,
                TypePlat = r.TypePlat != null ? r.TypePlat.ToString() : null,
                Difficulte = r.Difficulte != null ? r.Difficulte.ToString() : null
            })
            .ToListAsync();

        // --- Boissons (nom)
        result.Boissons = await _context.Boissons
            .Where(b => b.Nom.Contains(q))
            .OrderBy(b => b.Nom)
            .Take(limitPerCategory)
            .Select(b => new SearchBoissonItemDto
            {
                Id = b.IdBoisson,
                Nom = b.Nom,
                Type = b.TypeBoisson != null ? b.TypeBoisson.ToString() : null
            })
            .ToListAsync();

        // --- Ingrédients (uniquement ceux liés à ≥ 1 recette publiée, sinon résultats fantômes)
        result.Ingredients = await _context.Ingredients
            .Where(i => i.Nom.Contains(q)
                     && i.Recettes.Any(ri => ri.Recette!.Statut == StatutRecette.Publiee))
            .Select(i => new SearchIngredientItemDto
            {
                Id = i.IdIngredient,
                Nom = i.Nom,
                NombreRecettes = i.Recettes.Count(ri => ri.Recette!.Statut == StatutRecette.Publiee)
            })
            .OrderByDescending(i => i.NombreRecettes)
            .ThenBy(i => i.Nom)
            .Take(limitPerCategory)
            .ToListAsync();

        // --- Types de plat (enum à 3 valeurs : match en mémoire sur le libellé)
        var typesPlatMatches = Enum.GetValues<TypePlat>()
            .Where(tp => tp.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();

        foreach (var tp in typesPlatMatches)
        {
            var count = await _context.Recettes
                .CountAsync(r => r.Statut == StatutRecette.Publiee && r.TypePlat == tp);

            if (count > 0)
            {
                result.TypesPlat.Add(new SearchTypePlatItemDto
                {
                    Valeur = tp.ToString(),
                    NombreRecettes = count
                });
            }
        }

        result.TypesPlat = result.TypesPlat
            .OrderByDescending(t => t.NombreRecettes)
            .Take(limitPerCategory)
            .ToList();

        return result;
    }
}