using CulinaryPairing.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/recettes/{idRecette:int}/accords")]
public class AccordsController : ControllerBase
{
    private readonly IAccordsService _accordsService;

    public AccordsController(IAccordsService accordsService)
    {
        _accordsService = accordsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccords(int idRecette)
    {
        var accords = await _accordsService.GetAccordsByRecetteAsync(idRecette);
        return Ok(accords);
    }
}