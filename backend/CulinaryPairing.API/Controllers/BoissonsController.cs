using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CulinaryPairing.BLL.Services;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BoissonsController : ControllerBase
{
    private readonly IBoissonsService _boissonsService;
    private readonly IAccordsService _accordsService;

    public BoissonsController(
        IBoissonsService boissonsService,
        IAccordsService accordsService)
    {
        _boissonsService = boissonsService;
        _accordsService = accordsService;
    }

    /// <summary>
    /// Liste toutes les boissons (page /boissons côté front).
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var boissons = await _boissonsService.GetAllAsync();
        return Ok(boissons);
    }

    /// <summary>
    /// Détail d'une boisson.
    /// </summary>
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var boisson = await _boissonsService.GetByIdAsync(id);
        if (boisson is null)
            return NotFound();
        return Ok(boisson);
    }

    /// <summary>
    /// Accord inversé (V1.3) : recettes publiées qui s'accordent avec cette boisson,
    /// triées par score décroissant. Réutilise le moteur figé v1.3 et le cache ACCORD bidirectionnel.
    /// </summary>
    [HttpGet("{id}/accords")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAccords(int id, [FromQuery] int limit = 20)
    {
        // Validation des bornes en amont du service (le service re-clamp défensivement)
        if (limit < 1 || limit > 50)
            return BadRequest(new { error = "Le paramètre 'limit' doit être compris entre 1 et 50." });

        var accords = await _accordsService.GetRecettesByBoissonAsync(id, limit);

        // null = boisson inconnue (404), liste vide = boisson sans recette compatible (200 + [])
        if (accords is null)
            return NotFound();

        return Ok(accords);
    }
}