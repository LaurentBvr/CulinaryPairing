namespace CulinaryPairing.BLL.DTOs.Accords;

/// <summary>
/// Accord inversé : à partir d'une boisson, retourne la recette correspondante avec son score.
/// Distinct d'AccordDto (qui porte les champs boisson) — séparation des contrats API,
/// règle des trois, pas de mutualisation prématurée (CdC v1.3 §3.4).
/// </summary>
public class AccordInverseDto
{
    // === Champs ACCORD (miroir d'AccordDto) ===
    public int IdAccord { get; set; }
    public string? TypeAccord { get; set; }
    public string? Justification { get; set; }
    public int? ScoreCompatibilite { get; set; }
    public int? NiveauConfiance { get; set; }
    public int? MalusApplique { get; set; }
    public string? ReglesSatisfaites { get; set; }
    public DateTime DateCalcul { get; set; }
    public string? VersionMoteur { get; set; }

    // === Champs RECETTE (pour affichage card côté frontend, évite un appel supplémentaire) ===
    public int IdRecette { get; set; }
    public string Titre { get; set; } = "";
    public string? ImageUrl { get; set; }
    public string? TypePlat { get; set; }
    public string? Difficulte { get; set; }
    public int? TempsPreparation { get; set; }
    public int? TempsCuisson { get; set; }
}