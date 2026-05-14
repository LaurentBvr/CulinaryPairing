using CulinaryPairing.BLL.DTOs;

namespace CulinaryPairing.BLL.Services;

/// <summary>
/// Recherche transversale multi-entités (recettes, boissons, ingrédients, types de plat).
/// Filtre les recettes publiées uniquement (cohérent avec accords/listings publics).
/// Top-N par catégorie pour permettre dropdown live (limit=5) et page complète (limit=50).
/// </summary>
public interface ISearchService
{
    Task<SearchResultDto> SearchAsync(string query, int limitPerCategory = 5);
}