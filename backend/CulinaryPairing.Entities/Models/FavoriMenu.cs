using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("FAVORI_MENU")]
public class FavoriMenu
{
    [Column("id_utilisateur")]
    public int IdUtilisateur { get; set; }
    public Utilisateur Utilisateur { get; set; } = null!;

    [Column("id_menu")]
    public int IdMenu { get; set; }
    public MenuSoiree Menu { get; set; } = null!;

    [Column("date_ajout")]
    public DateTime DateAjout { get; set; } = DateTime.UtcNow;
}