using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinaryPairing.BLL.Substitution;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecettesController : ControllerBase
{
    private readonly CulinaryPairingDbContext _context;
    private readonly ISubstitutionService _substitutionService;

    public RecettesController(
        CulinaryPairingDbContext context,
        ISubstitutionService substitutionService)
    {
        _context = context;
        _substitutionService = substitutionService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var recettes = await _context.Recettes
            .Where(r => r.Statut == StatutRecette.Publiee)
            .Select(r => new
            {
                r.IdRecette,
                Nom = r.Titre,
                r.Description,
                TempsPreparation = r.TempsPreparation ?? 0,
                TempsCuisson = r.TempsCuisson ?? 0,
                NiveauDifficulte = r.Difficulte.ToString(),
                TypeRepas = r.TypePlat.ToString(),
                Categorie = r.TypePlat.ToString()
            })
            .ToListAsync();
        return Ok(recettes);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, [FromQuery] string? mode = null)
    {
        // R17/R18 : parse du mode d'adaptation (défaut = original, rétrocompat)
        ModeAdaptation modeAdaptation;
        if (string.IsNullOrEmpty(mode) || mode.Equals("original", StringComparison.OrdinalIgnoreCase))
            modeAdaptation = ModeAdaptation.Original;
        else if (mode.Equals("vegetarien", StringComparison.OrdinalIgnoreCase))
            modeAdaptation = ModeAdaptation.Vegetarien;
        else if (mode.Equals("vegan", StringComparison.OrdinalIgnoreCase))
            modeAdaptation = ModeAdaptation.Vegan;
        else
            return BadRequest(new { error = $"Mode invalide : '{mode}'. Valeurs acceptées : original, vegetarien, vegan." });

        // Charge la recette avec ingrédients + étapes (Include nécessaire pour passer au service)
        var recette = await _context.Recettes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Etapes)
            .FirstOrDefaultAsync(r => r.IdRecette == id && r.Statut == StatutRecette.Publiee);

        if (recette == null) return NotFound();

        // Substitutions chargées uniquement si nécessaire (économie en mode original)
        List<SubstitutionIngredient> substitutions;
        if (modeAdaptation == ModeAdaptation.Original)
        {
            substitutions = new List<SubstitutionIngredient>();
        }
        else
        {
            var idsIngredients = recette.Ingredients.Select(ri => ri.IdIngredient).ToList();
            substitutions = await _context.SubstitutionsIngredients
                .Include(s => s.IngredientSubstitut)
                .Where(s => idsIngredients.Contains(s.IdIngredientOriginal))
                .ToListAsync();
        }

        var adaptee = _substitutionService.AdapterRecette(recette.Ingredients, substitutions, modeAdaptation);

        return Ok(new
        {
            recette.IdRecette,
            Nom = recette.Titre,
            recette.Description,
            TempsPreparation = recette.TempsPreparation ?? 0,
            TempsCuisson = recette.TempsCuisson ?? 0,
            NiveauDifficulte = recette.Difficulte.ToString(),
            TypeRepas = recette.TypePlat.ToString(),
            Categorie = recette.TypePlat.ToString(),
            NombrePersonnesBase = recette.NombrePersonnesBase ?? 4,
            AdaptableVege = recette.AdaptableVege,
            AdaptableVegan = recette.AdaptableVegan,
            Mode = modeAdaptation.ToString().ToLowerInvariant(),
            Ingredients = adaptee.Ingredients
                .OrderBy(i => i.Nom)
                .Select(i => new
                {
                    i.IdIngredient,
                    i.Nom,
                    i.Quantite,
                    i.Unite,
                    i.EstVege,
                    i.EstVegan,
                    Substitut = i.Substitut == null ? null : new
                    {
                        i.Substitut.IdIngredient,
                        i.Substitut.Nom,
                        i.Substitut.QuantiteAdaptee,
                        i.Substitut.Unite,
                        i.Substitut.NoteCuisson
                    }
                }),
            IngredientsSansSubstitution = adaptee.IngredientsSansSubstitution,
            EstCompletementAdaptable = adaptee.EstCompletementAdaptable,
            Etapes = recette.Etapes
                .OrderBy(e => e.NumeroEtape)
                .Select(e => new { e.NumeroEtape, e.Description })
        });
    }
}