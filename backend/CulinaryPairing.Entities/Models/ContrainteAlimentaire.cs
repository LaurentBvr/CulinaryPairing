using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("CONTRAINTE_ALIMENTAIRE")]
public class ContrainteAlimentaire
{
    [Key]
    [Column("id_contrainte")]
    public int IdContrainte { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nom")]
    public string Nom { get; set; } = null!;

    [Required]
    [Column("type")]
    public TypeContrainte Type { get; set; }

    // Relations
    public ICollection<UtilisateurContrainte> Utilisateurs { get; set; } = new List<UtilisateurContrainte>();
    public ICollection<SoireeContrainte> Soirees { get; set; } = new List<SoireeContrainte>();
    public ICollection<IngredientContrainte> Ingredients { get; set; } = new List<IngredientContrainte>();
}