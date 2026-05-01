namespace CulinaryPairing.BLL.Favoris;

/// <summary>
/// DTO d'affichage d'un favori : recette + date d'ajout pour tri DESC.
/// </summary>
public class FavoriDto
{
    public int IdRecette { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? TypePlat { get; set; }
    public string? Difficulte { get; set; }
    public int? TempsPreparation { get; set; }
    public int? TempsCuisson { get; set; }
    public DateTime DateAjout { get; set; }
}