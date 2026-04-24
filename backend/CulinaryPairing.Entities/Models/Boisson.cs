using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("BOISSON")]
public class Boisson
{
    [Key]
    [Column("id_boisson")]
    public int IdBoisson { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nom")]
    public string Nom { get; set; } = null!;

    [Column("type_boisson")]
    public TypeBoisson? TypeBoisson { get; set; }

    [Column("alcoolise")]
    public bool Alcoolise { get; set; } = true;

    [Column("niveau_acidite")]
    public int? NiveauAcidite { get; set; }

    [Column("niveau_sucre")]
    public int? NiveauSucre { get; set; }

    [Column("niveau_tannins")]
    public int? NiveauTannins { get; set; }

    [Column("niveau_amertume")]
    public int? NiveauAmertume { get; set; }

    // V1.1
    [Column("degre_alcool", TypeName = "decimal(4,1)")]
    public decimal? DegreAlcool { get; set; }

    [Column("intensite_aromatique")]
    public int? IntensiteAromatique { get; set; }

    [Column("corps")]
    public CorpsBoisson? Corps { get; set; }

    // V1.2
    [Column("niveau_fume")]
    public int? NiveauFume { get; set; }

    [Column("temperature_optimale")]
    public int? TemperatureOptimale { get; set; }

    [Column("tolerance_temperature")]
    public int ToleranceTemperature { get; set; } = 2;

    [Column("cout_moyen", TypeName = "decimal(8,2)")]
    public decimal? CoutMoyen { get; set; }

    [Column("date_modification")]
    public DateTime? DateModification { get; set; }

    [MaxLength(50)]
    [Column("pays")]
    public string? Pays { get; set; }

    [MaxLength(100)]
    [Column("region")]
    public string? Region { get; set; }

    [MaxLength(100)]
    [Column("appellation")]
    public string? Appellation { get; set; }

    [MaxLength(100)]
    [Column("cepage")]
    public string? Cepage { get; set; }

    // Relations
    public ICollection<Accord> Accords { get; set; } = new List<Accord>();
    public ICollection<BoissonFamilleAromatique> FamillesAromatiques { get; set; } = new List<BoissonFamilleAromatique>();
}