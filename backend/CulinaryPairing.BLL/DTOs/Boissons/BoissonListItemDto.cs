namespace CulinaryPairing.BLL.DTOs.Boissons;

/// <summary>
/// DTO compact pour la page liste des boissons (/boissons).
/// Inclut les champs d'origine V1.3 utiles à l'affichage et au filtrage front.
/// </summary>
public class BoissonListItemDto
{
    public int IdBoisson { get; set; }
    public string Nom { get; set; } = "";
    public string? TypeBoisson { get; set; }
    public bool Alcoolise { get; set; }
    public string? Pays { get; set; }
    public string? Region { get; set; }
    public string? Appellation { get; set; }
    public string? Cepage { get; set; }
    public decimal? DegreAlcool { get; set; }
    public string? Corps { get; set; }
    public string? ImageUrl { get; set; }
}