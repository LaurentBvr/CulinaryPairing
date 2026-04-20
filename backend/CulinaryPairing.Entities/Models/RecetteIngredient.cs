using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("RECETTE_INGREDIENT")]
public class RecetteIngredient
{
    [Column("id_recette")]
    public int IdRecette { get; set; }
    public Recette Recette { get; set; } = null!;

    [Column("id_ingredient")]
    public int IdIngredient { get; set; }
    public Ingredient Ingredient { get; set; } = null!;

    [Column("quantite", TypeName = "decimal(10,2)")]
    public decimal Quantite { get; set; }
}