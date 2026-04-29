using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecettesController : ControllerBase
{
    private readonly CulinaryPairingDbContext _context;

    public RecettesController(CulinaryPairingDbContext context)
    {
        _context = context;
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
    public async Task<IActionResult> GetById(int id)
    {
        var recette = await _context.Recettes
            .Where(r => r.IdRecette == id && r.Statut == StatutRecette.Publiee)
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
                NombrePersonnesBase = r.NombrePersonnesBase ?? 4,
                AdaptableVege = r.AdaptableVege,
                AdaptableVegan = r.AdaptableVegan,
                Ingredients = r.Ingredients
                    .OrderBy(ri => ri.Ingredient.Nom)
                    .Select(ri => new
                    {
                        ri.IdIngredient,
                        Nom = ri.Ingredient.Nom,
                        Quantite = ri.Quantite,
                        Unite = ri.Ingredient.UniteDefaut,
                        EstVege = ri.Ingredient.EstVege,
                        EstVegan = ri.Ingredient.EstVegan
                    })
                    .ToList(),
                Etapes = r.Etapes
                    .OrderBy(e => e.NumeroEtape)
                    .Select(e => new
                    {
                        e.NumeroEtape,
                        e.Description
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (recette == null) return NotFound();
        return Ok(recette);
    }
}