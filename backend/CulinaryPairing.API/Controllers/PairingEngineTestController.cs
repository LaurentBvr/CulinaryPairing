using CulinaryPairing.BLL.PairingEngine;
using CulinaryPairing.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.API.Controllers;

/// <summary>
/// Endpoint TEMPORAIRE de diagnostic du moteur d'accords (étape 6 du plan).
/// À supprimer une fois l'étape 8 finalisée (intégration dans AccordsService + cache).
/// </summary>
[ApiController]
[Route("api/engine-test")]
[AllowAnonymous]
public class PairingEngineTestController : ControllerBase
{
    private readonly IPairingEngineService _engine;
    private readonly CulinaryPairingDbContext _db;

    public PairingEngineTestController(
        IPairingEngineService engine,
        CulinaryPairingDbContext db)
    {
        _engine = engine;
        _db = db;
    }

    [HttpGet("{recetteId:int}/{boissonId:int}")]
    public async Task<IActionResult> Tester(int recetteId, int boissonId)
    {
        // Eager loading des familles aromatiques pour R19bis et R25bis
        var recette = await _db.Recettes
            .Include(r => r.FamillesAromatiques)
                .ThenInclude(rf => rf.Famille)
            .FirstOrDefaultAsync(r => r.IdRecette == recetteId);

        var boisson = await _db.Boissons
            .Include(b => b.FamillesAromatiques)
                .ThenInclude(bf => bf.Famille)
            .FirstOrDefaultAsync(b => b.IdBoisson == boissonId);

        if (recette is null) return NotFound($"Recette {recetteId} introuvable.");
        if (boisson is null) return NotFound($"Boisson {boissonId} introuvable.");

        var resultat = _engine.CalculerScore(recette, boisson);

        return Ok(new
        {
            recette = recette.Titre,
            boisson = boisson.Nom,
            resultat
        });
    }
}