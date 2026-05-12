using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinaryPairing.BLL.Contraintes;
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
    private readonly IContraintesService _contraintesService;

    public RecettesController(
        CulinaryPairingDbContext context,
        ISubstitutionService substitutionService,
        IContraintesService contraintesService)
    {
        _context = context;
        _substitutionService = substitutionService;
        _contraintesService = contraintesService;
    }

    /// <summary>
    /// Lit l'id user depuis le JWT. Retourne null si anonyme ou claim non parsable.
    /// </summary>
    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
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
                Categorie = r.TypePlat.ToString(),
                r.AdaptableVege,
                r.AdaptableVegan
            })
            .ToListAsync();

        // R9/R16 : si user authentifié, calculer les contraintes violées en batch (1 seule requête)
        var userId = GetUserId();
        var violations = userId.HasValue
            ? await _contraintesService.GetContraintesVioleesAsync(
                userId.Value,
                recettes.Select(r => r.IdRecette).ToList())
            : new Dictionary<int, List<ContrainteDto>>();

        // R9 + R17/R18/R19 : enrichir avec contraintes violées ET statut ternaire de compatibilité
        var enrichies = recettes.Select(r =>
        {
            var contraintesViolees = violations.TryGetValue(r.IdRecette, out var v)
                ? v
                : new List<ContrainteDto>();
            var statut = CalculerStatutCompatibilite(contraintesViolees, r.AdaptableVege, r.AdaptableVegan);
            return new
            {
                r.IdRecette,
                r.Nom,
                r.Description,
                r.TempsPreparation,
                r.TempsCuisson,
                r.NiveauDifficulte,
                r.TypeRepas,
                r.Categorie,
                ContraintesViolees = contraintesViolees,
                StatutCompatibilite = statut
            };
        });

        return Ok(enrichies);
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

        var recette = await _context.Recettes
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.Etapes)
            .FirstOrDefaultAsync(r => r.IdRecette == id && r.Statut == StatutRecette.Publiee);

        if (recette == null) return NotFound();

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

        // R9/R16 : contraintes violées pour cette recette (vide si anonyme)
        var userId = GetUserId();
        var contraintesViolees = userId.HasValue
            ? (await _contraintesService.GetContraintesVioleesAsync(userId.Value, new List<int> { id }))
                .GetValueOrDefault(id, new List<ContrainteDto>())
            : new List<ContrainteDto>();

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
            ContraintesViolees = contraintesViolees,
            StatutCompatibilite = CalculerStatutCompatibilite(contraintesViolees, recette.AdaptableVege, recette.AdaptableVegan),
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

    /// <summary>
    /// R9 + R17/R18/R19 : statut ternaire de compatibilité d'une recette vis-à-vis des
    /// contraintes d'un utilisateur. Une recette est Adaptable si TOUTES les contraintes
    /// violées sont résolvables par substitution (Végétarien/Végan uniquement, sous réserve
    /// que la recette porte les flags AdaptableVege/AdaptableVegan). Les contraintes santé
    /// (allergies, sans gluten, sans lactose) et religieuses (halal, casher) ne sont jamais
    /// résolvables par le moteur de substitution V1.3.
    /// </summary>
    private static string CalculerStatutCompatibilite(
        List<ContrainteDto> contraintesViolees,
        bool adaptableVege,
        bool adaptableVegan)
    {
        if (contraintesViolees.Count == 0) return "Compatible";

        foreach (var c in contraintesViolees)
        {
            // Normalisation : "Végétarien" → "vegetarien" (insensible aux accents et à la casse)
            var normalise = new string(c.Nom
                .Normalize(System.Text.NormalizationForm.FormD)
                .Where(ch => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch)
                    != System.Globalization.UnicodeCategory.NonSpacingMark)
                .ToArray())
                .ToLowerInvariant();

            bool resolvable = normalise switch
            {
                "vegetarien" => adaptableVege,
                "vegan" => adaptableVegan,
                _ => false
            };

            if (!resolvable) return "Incompatible";
        }

        return "Adaptable";
    }
}