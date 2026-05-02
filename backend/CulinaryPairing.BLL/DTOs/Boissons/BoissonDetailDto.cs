namespace CulinaryPairing.BLL.DTOs.Boissons;

/// <summary>
/// DTO détaillé d'une boisson (page /boissons/:id).
/// Expose le profil gustatif complet, les paramètres de service et les familles aromatiques.
/// </summary>
public class BoissonDetailDto
{
    public int IdBoisson { get; set; }
    public string Nom { get; set; } = "";
    public string? TypeBoisson { get; set; }
    public bool Alcoolise { get; set; }

    // === Profil gustatif ===
    public int? NiveauAcidite { get; set; }
    public int? NiveauSucre { get; set; }
    public int? NiveauTannins { get; set; }
    public int? NiveauAmertume { get; set; }
    public int? IntensiteAromatique { get; set; }
    public int? NiveauFume { get; set; }
    public decimal? DegreAlcool { get; set; }
    public string? Corps { get; set; }

    // === Service (V1.2) ===
    public int? TemperatureOptimale { get; set; }
    public int? ToleranceTemperature { get; set; }

    // === Origine (V1.3) ===
    public string? Pays { get; set; }
    public string? Region { get; set; }
    public string? Appellation { get; set; }
    public string? Cepage { get; set; }

    public decimal? CoutMoyen { get; set; }

    // === Familles aromatiques associées (V1.1) ===
    public List<string> FamillesAromatiques { get; set; } = new();
}