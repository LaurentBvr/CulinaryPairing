using System.ComponentModel.DataAnnotations;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.BLL.DTOs.Soirees;

public class SoireeCreateDto
{
    [Range(2, 10)]
    public int NombrePersonnes { get; set; }

    [Range(0, 10)]
    public int NombreVegetariens { get; set; } = 0;

    [Range(0, 10)]
    public int NombreVegans { get; set; } = 0;

    public decimal? Budget { get; set; }
    public int? TempsDisponible { get; set; }
    public TypeSoiree? TypeSoiree { get; set; }
    public PreferenceAlcool PreferenceAlcool { get; set; } = PreferenceAlcool.Avec;

    public List<int> ContraintesIds { get; set; } = new();
}