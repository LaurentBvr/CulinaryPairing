using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("INGREDIENT_CONTRAINTE")]
public class IngredientContrainte
{
    [Column("id_ingredient")]
    public int IdIngredient { get; set; }
    public Ingredient Ingredient { get; set; } = null!;

    [Column("id_contrainte")]
    public int IdContrainte { get; set; }
    public ContrainteAlimentaire Contrainte { get; set; } = null!;
}