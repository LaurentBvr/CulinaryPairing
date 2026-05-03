
namespace CulinaryPairing.BLL.DTOs.Soirees;

public class MenuDto
{
    public int IdMenu { get; set; }
    public RecetteSlotDto? Entree { get; set; }
    public RecetteSlotDto? Plat { get; set; }
    public RecetteSlotDto? Dessert { get; set; }
    public decimal? CoutTotalEstime { get; set; }
    public int? TempsTotalEstime { get; set; }
}