using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("FAVORI")]
public class Favori
{
    [Column("id_utilisateur")]
    public int IdUtilisateur { get; set; }
    public Utilisateur Utilisateur { get; set; } = null!;

    [Column("id_recette")]
    public int IdRecette { get; set; }
    public Recette Recette { get; set; } = null!;

    [Column("date_ajout")]
    public DateTime DateAjout { get; set; } = DateTime.UtcNow;
}