using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("ETAPE")]
public class Etape
{
    [Key]
    [Column("id_etape")]
    public int IdEtape { get; set; }

    [Required]
    [Column("numero_etape")]
    public int NumeroEtape { get; set; }

    [Required]
    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("id_recette")]
    public int IdRecette { get; set; }
    public Recette Recette { get; set; } = null!;
}