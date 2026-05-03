using CulinaryPairing.BLL.Contraintes;

namespace CulinaryPairing.BLL.DTOs.Soirees;

public class SoireeDetailDto
{
    public int IdSoiree { get; set; }
    public int NombrePersonnes { get; set; }
    public int NombreVegetariens { get; set; }
    public int NombreVegans { get; set; }
    public decimal? Budget { get; set; }
    public int? TempsDisponible { get; set; }
    public string? TypeSoiree { get; set; }
    public string PreferenceAlcool { get; set; } = "";
    public DateTime DateCreation { get; set; }

    public List<ContrainteDto> ContraintesAgregees { get; set; } = new();
    public MenuDto? Menu { get; set; }
}