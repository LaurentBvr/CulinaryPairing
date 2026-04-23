namespace CulinaryPairing.BLL.DTOs.Accords;

public class AccordDto
{
    public int IdAccord { get; set; }
    public string? TypeAccord { get; set; }
    public string? Justification { get; set; }
    public int? ScoreCompatibilite { get; set; }
    public int? NiveauConfiance { get; set; }
    public int? MalusApplique { get; set; }
    public string? ReglesSatisfaites { get; set; }
    public int IdBoisson { get; set; }
    public string? NomBoisson { get; set; }
    public string? TypeBoisson { get; set; }
}