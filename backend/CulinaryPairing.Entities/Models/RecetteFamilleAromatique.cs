using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("RECETTE_FAMILLE_AROMATIQUE")]
public class RecetteFamilleAromatique
{
    [Column("id_recette")]
    public int IdRecette { get; set; }
    public Recette Recette { get; set; } = null!;

    [Column("id_famille")]
    public int IdFamille { get; set; }
    public FamilleAromatique Famille { get; set; } = null!;
}