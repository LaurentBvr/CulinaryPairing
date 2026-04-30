namespace CulinaryPairing.BLL.Substitution;

public class IngredientAdapteDto
{
    public int IdIngredient { get; set; }
    public string Nom { get; set; } = string.Empty;
    public decimal Quantite { get; set; }
    public string? Unite { get; set; }
    public bool EstVege { get; set; }
    public bool EstVegan { get; set; }
    public SubstitutDto? Substitut { get; set; }
}

public class SubstitutDto
{
    public int IdIngredient { get; set; }
    public string Nom { get; set; } = string.Empty;
    public decimal QuantiteAdaptee { get; set; }
    public string? Unite { get; set; }
    public string? NoteCuisson { get; set; }
}

public class RecetteAdapteeDto
{
    public ModeAdaptation Mode { get; set; }
    public List<IngredientAdapteDto> Ingredients { get; set; } = new();
    public List<string> IngredientsSansSubstitution { get; set; } = new();
    public bool EstCompletementAdaptable => IngredientsSansSubstitution.Count == 0;
}