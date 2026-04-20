using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinaryPairing.Entities.Models;

[Table("REPONSE_QUIZ")]
public class ReponseQuiz
{
    [Key]
    [Column("id_reponse")]
    public int IdReponse { get; set; }

    [Column("id_question")]
    public int IdQuestion { get; set; }
    public QuestionQuiz Question { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("texte_reponse")]
    public string TexteReponse { get; set; } = null!;

    [Column("est_correcte")]
    public bool EstCorrecte { get; set; } = false;
}