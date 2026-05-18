namespace CulinaryPairing.BLL.DTOs;

/// <summary>
/// DTO retourné par GET /api/historique/mes-dernieres.
/// Représente une recette unique consultée par l'utilisateur,
/// projetée avec la date de sa dernière consultation.
/// </summary>
public class HistoriqueRecetteDto
{
    public int IdRecette { get; set; }
    public string Titre { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string TypePlat { get; set; } = string.Empty;
    public DateTime DerniereConsultation { get; set; }
}