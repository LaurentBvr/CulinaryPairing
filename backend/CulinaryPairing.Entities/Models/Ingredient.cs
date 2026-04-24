using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("INGREDIENT")]
public class Ingredient
{
    [Key]
    [Column("id_ingredient")]
    public int IdIngredient { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nom")]
    public string Nom { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    [Column("unite_defaut")]
    public string UniteDefaut { get; set; } = null!;

    [Column("est_allergene")]
    public bool EstAllergene { get; set; } = false;

    [Column("est_alcool")]
    public bool EstAlcool { get; set; } = false;

    [Column("est_vege")]
    public bool EstVege { get; set; } = true;

    [Column("est_vegan")]
    public bool EstVegan { get; set; } = true;

    [Column("divisible")]
    public bool Divisible { get; set; } = true;

    [Column("cout_unitaire", TypeName = "decimal(8,2)")]
    public decimal? CoutUnitaire { get; set; }

    [Column("date_maj_prix")]
    public DateTime? DateMajPrix { get; set; }

    // Relations
    public ICollection<RecetteIngredient> Recettes { get; set; } = new List<RecetteIngredient>();
    public ICollection<SubstitutionIngredient> SubstitutionsOriginales { get; set; } = new List<SubstitutionIngredient>();
    public ICollection<SubstitutionIngredient> SubstitutionsSubstituts { get; set; } = new List<SubstitutionIngredient>();
    public ICollection<IngredientContrainte> Contraintes { get; set; } = new List<IngredientContrainte>();
}