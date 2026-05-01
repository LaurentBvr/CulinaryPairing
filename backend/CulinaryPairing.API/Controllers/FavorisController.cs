using System.Security.Claims;
using CulinaryPairing.BLL.Favoris;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/favoris")]
[Authorize]
public class FavorisController : ControllerBase
{
    private readonly IFavorisService _favorisService;

    public FavorisController(IFavorisService favorisService)
    {
        _favorisService = favorisService;
    }

    /// <summary>
    /// Extrait l'id utilisateur du JWT. Retourne null si claim absent ou non parsable.
    /// </summary>
    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    // GET /api/favoris → liste des favoris de l'utilisateur (DESC date_ajout)
    [HttpGet]
    public async Task<ActionResult<List<FavoriDto>>> GetMesFavoris()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var favoris = await _favorisService.GetByUserAsync(userId.Value);
        return Ok(favoris);
    }

    // GET /api/favoris/ids → set d'ids (optimisation front : 1 appel pour annoter une liste de recettes)
    [HttpGet("ids")]
    public async Task<ActionResult<int[]>> GetMesFavorisIds()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var ids = await _favorisService.GetIdsRecettesAsync(userId.Value);
        return Ok(ids.ToArray());
    }

    // GET /api/favoris/{idRecette}/check → bool (utile fiche recette détail)
    [HttpGet("{idRecette:int}/check")]
    public async Task<ActionResult<bool>> CheckFavori(int idRecette)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var isFav = await _favorisService.IsFavoriAsync(userId.Value, idRecette);
        return Ok(isFav);
    }

    // POST /api/favoris/{idRecette} → ajouter (idempotent)
    [HttpPost("{idRecette:int}")]
    public async Task<IActionResult> AjouterFavori(int idRecette)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        try
        {
            var added = await _favorisService.AddAsync(userId.Value, idRecette);
            // 201 si nouvellement ajouté, 200 si déjà présent (idempotent)
            return added ? StatusCode(201) : Ok(new { message = "Déjà en favori." });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = "Recette introuvable." });
        }
    }

    // DELETE /api/favoris/{idRecette} → retirer
    [HttpDelete("{idRecette:int}")]
    public async Task<IActionResult> RetirerFavori(int idRecette)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var removed = await _favorisService.RemoveAsync(userId.Value, idRecette);
        return removed ? NoContent() : NotFound(new { message = "Favori introuvable." });
    }
}