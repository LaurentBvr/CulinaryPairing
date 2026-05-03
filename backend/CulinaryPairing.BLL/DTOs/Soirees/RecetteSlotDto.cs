namespace CulinaryPairing.BLL.DTOs.Soirees;

public class RecetteSlotDto
{
    public int IdRecette { get; set; }
    public string Titre { get; set; } = "";
    public string? ImageUrl { get; set; }
    public string? TypePlat { get; set; }
    public int? TempsPreparation { get; set; }
    public int? TempsCuisson { get; set; }
    public decimal? CoutEstime { get; set; }
}