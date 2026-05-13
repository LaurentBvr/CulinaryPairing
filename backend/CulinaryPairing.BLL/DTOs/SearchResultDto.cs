namespace CulinaryPairing.BLL.DTOs;

public class SearchResultDto
{
    public List<SearchRecetteItemDto> Recettes { get; set; } = new();
    public List<SearchBoissonItemDto> Boissons { get; set; } = new();
    public List<SearchIngredientItemDto> Ingredients { get; set; } = new();
    public List<SearchTypePlatItemDto> TypesPlat { get; set; } = new();
    public int TotalResultats => Recettes.Count + Boissons.Count + Ingredients.Count + TypesPlat.Count;
}

public class SearchRecetteItemDto
{
    public int Id { get; set; }
    public string Titre { get; set; } = "";
    public string? TypePlat { get; set; }
    public string? Difficulte { get; set; }
}

public class SearchBoissonItemDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = "";
    public string? Type { get; set; }
}

public class SearchIngredientItemDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = "";
    public int NombreRecettes { get; set; }
}

public class SearchTypePlatItemDto
{
    public string Valeur { get; set; } = "";
    public int NombreRecettes { get; set; }
}