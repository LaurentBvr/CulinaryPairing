using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("MENU_SOIREE")]
public class MenuSoiree
{
    [Key]
    [Column("id_menu")]
    public int IdMenu { get; set; }

    [Column("id_soiree")]
    public int IdSoiree { get; set; }
    public Soiree Soiree { get; set; } = null!;

    [Column("id_recette_entree")]
    public int? IdRecetteEntree { get; set; }
    public Recette? RecetteEntree { get; set; }

    [Column("id_recette_plat")]
    public int? IdRecettePlat { get; set; }
    public Recette? RecettePlat { get; set; }

    [Column("id_recette_dessert")]
    public int? IdRecetteDessert { get; set; }
    public Recette? RecetteDessert { get; set; }

    [Column("id_boisson_entree")]
    public int? IdBoissonEntree { get; set; }
    public Boisson? BoissonEntree { get; set; }

    [Column("id_boisson_plat")]
    public int? IdBoissonPlat { get; set; }
    public Boisson? BoissonPlat { get; set; }

    [Column("id_boisson_dessert")]
    public int? IdBoissonDessert { get; set; }
    public Boisson? BoissonDessert { get; set; }

    [Column("cout_total_estime", TypeName = "decimal(8,2)")]
    public decimal? CoutTotalEstime { get; set; }

    [Column("temps_total_estime")]
    public int? TempsTotalEstime { get; set; }
}