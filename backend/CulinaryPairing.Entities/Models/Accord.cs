using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("ACCORD")]
public class Accord
{
    [Key]
    [Column("id_accord")]
    public int IdAccord { get; set; }

    [Column("type_accord")]
    public TypeAccord? TypeAccord { get; set; }

    [Column("justification")]
    public string? Justification { get; set; }

    [Column("score_compatibilite")]
    public int? ScoreCompatibilite { get; set; }

    [Column("niveau_confiance")]
    public int? NiveauConfiance { get; set; }

    [MaxLength(500)]
    [Column("regles_satisfaites")]
    public string? ReglesSatisfaites { get; set; }

    [Column("malus_applique")]
    public int? MalusApplique { get; set; }

    [Column("id_recette")]
    public int IdRecette { get; set; }
    public Recette Recette { get; set; } = null!;

    [Column("id_boisson")]
    public int IdBoisson { get; set; }
    public Boisson Boisson { get; set; } = null!;
}