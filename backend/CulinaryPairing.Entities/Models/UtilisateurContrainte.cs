using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("UTILISATEUR_CONTRAINTE")]
public class UtilisateurContrainte
{
    [Column("id_utilisateur")]
    public int IdUtilisateur { get; set; }
    public Utilisateur Utilisateur { get; set; } = null!;

    [Column("id_contrainte")]
    public int IdContrainte { get; set; }
    public ContrainteAlimentaire Contrainte { get; set; } = null!;
}