using CulinaryPairing.BLL.DTOs;
using CulinaryPairing.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

/// <summary>
/// Recherche transversale multi-entités (recettes publiées, boissons, ingrédients, types de plat).
/// Endpoint unique consommé par :
///   - le dropdown live de la navbar (limit=5 par catégorie)
///   - la page /recherche complète (limit=50 par catégorie)
/// Pas d'authentification requise : la recherche est un service public au même titre
/// que les listings de recettes/boissons.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController(ISearchService searchService)
    {
        _searchService = searchService;
    }

    /// <summary>
    /// GET /api/search?q=tomate&limit=5
    /// </summary>
    /// <param name="q">Requête de recherche (min 2 caractères, sinon résultat vide).</param>
    /// <param name="limit">Nombre max de résultats par catégorie (1-50, défaut 5).</param>
    [HttpGet]
    public async Task<ActionResult<SearchResultDto>> Search(
        [FromQuery] string? q,
        [FromQuery] int limit = 5)
    {
        // Validation défensive : le service gère déjà le clamp, mais on évite l'appel inutile
        if (string.IsNullOrWhiteSpace(q))
            return Ok(new SearchResultDto());

        if (limit < 1) limit = 1;
        if (limit > 50) limit = 50;

        var result = await _searchService.SearchAsync(q, limit);
        return Ok(result);
    }
}