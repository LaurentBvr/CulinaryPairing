using CulinaryPairing.BLL.DTOs.Accords;

namespace CulinaryPairing.BLL.Services;

public interface IAccordsService
{
    Task<List<AccordDto>> GetAccordsByRecetteAsync(int idRecette);
}