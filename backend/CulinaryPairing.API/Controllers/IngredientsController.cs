using CulinaryPairing.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController(CulinaryPairingDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string q = "")
    {
        var ingredients = await db.Ingredients
            .Where(i => i.Nom.Contains(q))
            .OrderBy(i => i.Nom)
            .Take(10)
            .Select(i => new { i.IdIngredient, i.Nom })
            .ToListAsync();

        return Ok(ingredients);
    }

    [HttpGet("{id}/info")]
    public async Task<IActionResult> Info(int id)
    {
    var ingredient = await db.Ingredients.FindAsync(id);
    if (ingredient == null) return NotFound();

    var recettes = await db.RecettesIngredients
        .Where(ri => ri.IdIngredient == id
                  && ri.Recette.Statut == StatutRecette.Publiee)
        .Select(ri => ri.Recette.Ingredients.Count)
        .ToListAsync();

    if (recettes.Count == 0)
        return Ok(new { recettesCount = 0, minIngredients = 0, maxIngredients = 0 });

    return Ok(new
    {
        recettesCount = recettes.Count,
        minIngredients = recettes.Min(),
        maxIngredients = recettes.Max()
    });
    }
}