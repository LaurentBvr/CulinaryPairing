using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("RECETTE")]
public class Recette
{
    [Key]
    [Column("id_recette")]
    public int IdRecette { get; set; }

    [Required]
    [MaxLength(200)]
    [Column("titre")]
    public string Titre { get; set; } = null!;

    [Column("description")]
    public string? Description { get; set; }

    [MaxLength(500)]
    [Column("image_url")]
    public string ImageUrl { get; set; } = "/images/placeholder.jpg";

    [Column("temps_preparation")]
    public int? TempsPreparation { get; set; }

    [Column("temps_cuisson")]
    public int? TempsCuisson { get; set; }

    [Column("difficulte")]
    public Difficulte? Difficulte { get; set; }

    [Column("type_plat")]
    public TypePlat? TypePlat { get; set; }

    [Column("nombre_personnes_base")]
    public int? NombrePersonnesBase { get; set; }

    // Caractéristiques gustatives V1.0
    [Column("niveau_gras")]
    public int? NiveauGras { get; set; }

    [Column("niveau_acidite")]
    public int? NiveauAcidite { get; set; }

    [Column("niveau_piquant")]
    public int? NiveauPiquant { get; set; }

    [Column("niveau_arome_epice")]
    public int? NiveauAromeEpice { get; set; }

    [Column("niveau_umami")]
    public int? NiveauUmami { get; set; }

    // V1.1 - Moteur d'accords enrichi
    [Column("niveau_sucre")]
    public int? NiveauSucre { get; set; }

    [Column("niveau_sel")]
    public int? NiveauSel { get; set; }

    [Column("intensite_aromatique")]
    public int? IntensiteAromatique { get; set; }

    [Column("contient_umami_pur")]
    public bool ContientUmamiPur { get; set; } = false;

    [Column("contient_fume")]
    public bool ContientFume { get; set; } = false;

    // V1.2 - Préparation et cohérence conceptuelle
    [Column("affinite_tannins")]
    public AffiniteTannins AffiniteTannins { get; set; } = AffiniteTannins.Neutre;

    [Column("mode_cuisson")]
    public ModeCuisson? ModeCuisson { get; set; }

    [Column("type_sauce")]
    public TypeSauce? TypeSauce { get; set; }

    [Column("est_publiee")]
    public bool EstPubliee { get; set; } = false;

    [Column("adaptable_vege")]
    public bool AdaptableVege { get; set; } = false;

    [Column("adaptable_vegan")]
    public bool AdaptableVegan { get; set; } = false;

    [Column("cout_estime", TypeName = "decimal(8,2)")]
    public decimal? CoutEstime { get; set; }

    [Column("preparable_avance")]
    public bool PreparableAvance { get; set; } = false;

    [Column("temps_finition")]
    public int? TempsFinition { get; set; }

    [Column("date_creation")]
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    [Column("id_utilisateur")]
    public int? IdUtilisateur { get; set; }
    public Utilisateur? Utilisateur { get; set; }

    // Relations
    public ICollection<RecetteIngredient> Ingredients { get; set; } = new List<RecetteIngredient>();
    public ICollection<Etape> Etapes { get; set; } = new List<Etape>();
    public ICollection<RecetteFamilleAromatique> FamillesAromatiques { get; set; } = new List<RecetteFamilleAromatique>();
    public ICollection<Accord> Accords { get; set; } = new List<Accord>();
    public ICollection<Favori> Favoris { get; set; } = new List<Favori>();
    public ICollection<HistoriqueConsultation> Historiques { get; set; } = new List<HistoriqueConsultation>();
}