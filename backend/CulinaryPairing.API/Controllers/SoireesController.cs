using System.Security.Claims;
using CulinaryPairing.BLL.DTOs.Soirees;
using CulinaryPairing.BLL.Soirees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/soirees")]
[Authorize]
public class SoireesController : ControllerBase
{
    private readonly ISoireesService _service;

    public SoireesController(ISoireesService service) => _service = service;

    /// <summary>Récupère l'id user depuis le claim JWT. Levée si absent (devrait jamais arriver avec [Authorize]).</summary>
    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new UnauthorizedAccessException("Claim user manquant.");
        return int.Parse(claim);
    }

    // GET /api/soirees
    [HttpGet]
    public async Task<ActionResult<List<SoireeListItemDto>>> GetMine()
        => Ok(await _service.GetMineAsync(GetUserId()));

    // GET /api/soirees/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SoireeDetailDto>> GetById(int id)
    {
        var soiree = await _service.GetByIdAsync(id, GetUserId());
        return soiree == null ? NotFound() : Ok(soiree);
    }

    // POST /api/soirees
    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody] SoireeCreateDto dto)
    {
        try
        {
            var idSoiree = await _service.CreateAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetById), new { id = idSoiree }, idSoiree);
        }
        catch (ArgumentException ex)
        {
            // Défense en profondeur : 400 propre avant que le CHECK SQL ne lève une SqlException.
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT /api/soirees/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] SoireeUpdateDto dto)
    {
        try
        {
            var ok = await _service.UpdateAsync(id, GetUserId(), dto);
            return ok ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // DELETE /api/soirees/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteAsync(id, GetUserId());
        return ok ? NoContent() : NotFound();
    }
}