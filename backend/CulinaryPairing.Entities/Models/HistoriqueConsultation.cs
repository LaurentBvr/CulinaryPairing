using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("HISTORIQUE_CONSULTATION")]
public class HistoriqueConsultation
{
    [Key]
    [Column("id_historique")]
    public int IdHistorique { get; set; }

    [Column("id_utilisateur")]
    public int IdUtilisateur { get; set; }
    public Utilisateur Utilisateur { get; set; } = null!;

    [Column("id_recette")]
    public int IdRecette { get; set; }
    public Recette Recette { get; set; } = null!;

    [Column("date_consultation")]
    public DateTime DateConsultation { get; set; } = DateTime.UtcNow;
}