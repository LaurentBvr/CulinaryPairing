using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("UTILISATEUR")]
public class Utilisateur
{
    [Key]
    [Column("id_utilisateur")]
    public int IdUtilisateur { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nom")]
    public string Nom { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    [Column("prenom")]
    public string Prenom { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("email")]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("mot_de_passe")]
    public string MotDePasse { get; set; } = null!;

    [Column("role")]
    public RoleUtilisateur Role { get; set; } = RoleUtilisateur.Utilisateur;

    [Column("preferences_alcool")]
    public bool PreferencesAlcool { get; set; } = true;

    [Column("regime_defaut")]
    public RegimeAlimentaire RegimeDefaut { get; set; } = RegimeAlimentaire.Omnivore;

    [Column("niveau_cuisine")]
    public NiveauCuisine NiveauCuisine { get; set; } = NiveauCuisine.Debutant;

    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Relations
    public ICollection<UtilisateurContrainte> Contraintes { get; set; } = new List<UtilisateurContrainte>();
    public ICollection<Recette> RecettesCreees { get; set; } = new List<Recette>();
    public ICollection<Favori> Favoris { get; set; } = new List<Favori>();
    public ICollection<HistoriqueConsultation> Historique { get; set; } = new List<HistoriqueConsultation>();
    public ICollection<ScoreQuiz> Scores { get; set; } = new List<ScoreQuiz>();
    public ICollection<Soiree> Soirees { get; set; } = new List<Soiree>();
}