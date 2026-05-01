using System.Security.Claims;
using CulinaryPairing.BLL.Contraintes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/contraintes")]
public class ContraintesController : ControllerBase
{
    private readonly IContraintesService _contraintesService;

    public ContraintesController(IContraintesService contraintesService)
    {
        _contraintesService = contraintesService;
    }

    private int? GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    /// <summary>Catalogue public des contraintes alimentaires (8 entrées V1.3).</summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<ContrainteDto>>> GetCatalogue()
    {
        var catalogue = await _contraintesService.GetAllAsync();
        return Ok(catalogue);
    }

    /// <summary>Contraintes activées par l'utilisateur courant.</summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<List<ContrainteDto>>> GetMesContraintes()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        var mesContraintes = await _contraintesService.GetByUserAsync(userId.Value);
        return Ok(mesContraintes);
    }

    /// <summary>Remplace les contraintes activées de l'utilisateur courant (R9 + R16 source).</summary>
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateMesContraintes([FromBody] UpdateContraintesDto dto)
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized();

        try
        {
            await _contraintesService.UpdateUserContraintesAsync(
                userId.Value,
                dto.IdsContraintes ?? new List<int>());
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}