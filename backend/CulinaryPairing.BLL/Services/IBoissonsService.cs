using CulinaryPairing.BLL.DTOs.Boissons;

namespace CulinaryPairing.BLL.Services;

/// <summary>
/// Service de lecture sur les boissons. Séparé d'AccordsService (SRP) :
/// les accords orchestrent moteur + cache, BoissonsService fait du CRUD lecture pur.
/// Symétrique de RecettesService côté recettes.
/// </summary>
public interface IBoissonsService
{
    Task<List<BoissonListItemDto>> GetAllAsync();
    Task<BoissonDetailDto?> GetByIdAsync(int idBoisson);
}