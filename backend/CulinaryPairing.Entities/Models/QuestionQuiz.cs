using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("QUESTION_QUIZ")]
public class QuestionQuiz
{
    [Key]
    [Column("id_question")]
    public int IdQuestion { get; set; }

    [Required]
    [Column("texte_question")]
    public string TexteQuestion { get; set; } = null!;

    [Required]
    [Column("difficulte")]
    public DifficulteQuiz Difficulte { get; set; }

    [Required]
    [Column("explication")]
    public string Explication { get; set; } = null!;

    [Column("id_recette_exemple")]
    public int? IdRecetteExemple { get; set; }
    public Recette? RecetteExemple { get; set; }

    // Relations
    public ICollection<ReponseQuiz> Reponses { get; set; } = new List<ReponseQuiz>();
}