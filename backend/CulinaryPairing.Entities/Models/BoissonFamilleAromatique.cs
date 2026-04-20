using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("BOISSON_FAMILLE_AROMATIQUE")]
public class BoissonFamilleAromatique
{
    [Column("id_boisson")]
    public int IdBoisson { get; set; }
    public Boisson Boisson { get; set; } = null!;

    [Column("id_famille")]
    public int IdFamille { get; set; }
    public FamilleAromatique Famille { get; set; } = null!;
}