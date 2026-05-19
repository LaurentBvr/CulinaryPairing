using System.Security.Claims;
using CulinaryPairing.BLL.DTOs;
using CulinaryPairing.BLL.Historique;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/historique")]
[Authorize]
public class HistoriqueController : ControllerBase
{
    private readonly IHistoriqueService _historiqueService;

    public HistoriqueController(IHistoriqueService historiqueService)
    {
        _historiqueService = historiqueService;
    }

    public class EnregistrerConsultationRequest
    {
        public int IdRecette { get; set; }
    }

    /// <summary>
    /// Enregistre la consultation d'une recette par l'utilisateur courant.
    /// Renvoie 200 OK que l'événement ait été persisté ou dédupliqué :
    /// du point de vue du client, la consultation est "prise en compte".
    /// </summary>
    [HttpPost("consultation")]
    public async Task<IActionResult> EnregistrerConsultation(
        [FromBody] EnregistrerConsultationRequest request,
        CancellationToken ct)
    {
        if (request is null || request.IdRecette <= 0)
        {
            return BadRequest(new { message = "IdRecette invalide." });
        }

        var idUtilisateur = GetUserId();
        if (idUtilisateur is null)
        {
            return Unauthorized();
        }

        var inseree = await _historiqueService.AjouterAsync(idUtilisateur.Value, request.IdRecette, ct);
        return Ok(new { enregistree = inseree });
    }

    /// <summary>
    /// Retourne les N dernières recettes uniques consultées par l'utilisateur courant.
    /// Limite plafonnée à 20 côté service.
    /// </summary>
    [HttpGet("mes-dernieres")]
    public async Task<ActionResult<IReadOnlyList<HistoriqueRecetteDto>>> MesDernieres(
        [FromQuery] int limit = 5,
        CancellationToken ct = default)
    {
        var idUtilisateur = GetUserId();
        if (idUtilisateur is null)
        {
            return Unauthorized();
        }

        var dernieres = await _historiqueService.GetDernieresRecettesAsync(idUtilisateur.Value, limit, ct);
        return Ok(dernieres);
    }

    private int? GetUserId()
    {
    var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return int.TryParse(claim, out var id) ? id : null;
    }
}