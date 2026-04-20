using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("FAMILLE_AROMATIQUE")]
public class FamilleAromatique
{
    [Key]
    [Column("id_famille")]
    public int IdFamille { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("nom")]
    public string Nom { get; set; } = null!;

    [MaxLength(255)]
    [Column("description")]
    public string? Description { get; set; }

    // Relations
    public ICollection<RecetteFamilleAromatique> Recettes { get; set; } = new List<RecetteFamilleAromatique>();
    public ICollection<BoissonFamilleAromatique> Boissons { get; set; } = new List<BoissonFamilleAromatique>();
}