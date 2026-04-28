using CulinaryPairing.BLL.VideFrigo;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/vide-frigo")]
public class VideFrigoController(VideFrigoService videFrigoService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Rechercher([FromBody] VideFrigoRequestDto req)
    {
        if (req.IngredientIds.Count == 0)
            return BadRequest("Au moins un ingrédient requis.");

        var results = await videFrigoService.RechercherAsync(req);
        return Ok(results);
    }
}