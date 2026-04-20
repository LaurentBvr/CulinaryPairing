using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("SOIREE")]
public class Soiree
{
    [Key]
    [Column("id_soiree")]
    public int IdSoiree { get; set; }

    [Column("id_utilisateur")]
    public int IdUtilisateur { get; set; }
    public Utilisateur Utilisateur { get; set; } = null!;

    [Column("nombre_personnes")]
    public int? NombrePersonnes { get; set; }

    [Column("nombre_vegetariens")]
    public int NombreVegetariens { get; set; } = 0;

    [Column("nombre_vegans")]
    public int NombreVegans { get; set; } = 0;

    [Column("budget", TypeName = "decimal(8,2)")]
    public decimal? Budget { get; set; }

    [Column("temps_disponible")]
    public int? TempsDisponible { get; set; }

    [Column("type_soiree")]
    public TypeSoiree? TypeSoiree { get; set; }

    [Column("preference_alcool")]
    public PreferenceAlcool PreferenceAlcool { get; set; } = PreferenceAlcool.Avec;

    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Relations
    public ICollection<SoireeContrainte> Contraintes { get; set; } = new List<SoireeContrainte>();
    public ICollection<MenuSoiree> Menus { get; set; } = new List<MenuSoiree>();
}