using CulinaryPairing.BLL.DTOs.Accords;

namespace CulinaryPairing.BLL.Services;

public interface IAccordsService
{
    /// <summary>
    /// Sens direct : à partir d'une recette, retourne les boissons triées par score décroissant.
    /// </summary>
    Task<List<AccordDto>> GetAccordsByRecetteAsync(int idRecette);

    /// <summary>
    /// Sens inversé (V1.3) : à partir d'une boisson, retourne les recettes publiées qui
    /// s'accordent le mieux, triées par score décroissant.
    /// Réutilise le même moteur (IPairingEngineService.CalculerScore) et le même cache
    /// ACCORD bidirectionnel (clé unique recette/boisson/type_accord).
    /// </summary>
    /// <param name="idBoisson">Identifiant de la boisson.</param>
    /// <param name="limit">Nombre max de recettes retournées (1-50, défaut 20).</param>
    /// <returns>
    ///   - null si la boisson n'existe pas (→ 404 controller).
    ///   - Liste vide si aucune recette publiée n'est compatible.
    ///   - Liste triée sinon.
    /// </returns>
    Task<List<AccordInverseDto>?> GetRecettesByBoissonAsync(int idBoisson, int limit = 20);
}