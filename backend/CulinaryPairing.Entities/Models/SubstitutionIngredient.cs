using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("SUBSTITUTION_INGREDIENT")]
public class SubstitutionIngredient
{
    [Key]
    [Column("id_substitution")]
    public int IdSubstitution { get; set; }

    [Column("id_ingredient_original")]
    public int IdIngredientOriginal { get; set; }
    public Ingredient IngredientOriginal { get; set; } = null!;

    [Column("id_ingredient_substitut")]
    public int IdIngredientSubstitut { get; set; }
    public Ingredient IngredientSubstitut { get; set; } = null!;

    [Required]
    [Column("type_substitution")]
    public TypeSubstitution TypeSubstitution { get; set; }

    [Column("ratio_conversion", TypeName = "decimal(4,2)")]
    public decimal RatioConversion { get; set; } = 1.0m;

    [MaxLength(500)]
    [Column("note_cuisson")]
    public string? NoteCuisson { get; set; }
}