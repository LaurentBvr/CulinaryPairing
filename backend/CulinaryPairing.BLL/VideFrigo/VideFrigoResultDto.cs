namespace CulinaryPairing.BLL.VideFrigo;

public class VideFrigoResultDto
{
    public int RecetteId { get; set; }
    public string Titre { get; set; } = "";
    public int Score { get; set; }
    public List<string> IngredientsPresents { get; set; } = new();
    public List<string> IngredientsManquants { get; set; } = new();
    public bool BadgeVeg { get; set; }
}