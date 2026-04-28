namespace CulinaryPairing.BLL.VideFrigo;

public class VideFrigoRequestDto
{
    public List<int> IngredientIds { get; set; } = new();
    public int NombreResultats { get; set; } = 10; // 5, 10, 15 ou 20
    public bool InclureVeg { get; set; } = false;
}