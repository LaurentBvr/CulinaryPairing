using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("SOIREE_CONTRAINTE")]
public class SoireeContrainte
{
    [Column("id_soiree")]
    public int IdSoiree { get; set; }
    public Soiree Soiree { get; set; } = null!;

    [Column("id_contrainte")]
    public int IdContrainte { get; set; }
    public ContrainteAlimentaire Contrainte { get; set; } = null!;
}