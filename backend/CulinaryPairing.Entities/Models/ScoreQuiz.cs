using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinaryPairing.Entities.Enums;

namespace CulinaryPairing.Entities.Models;

[Table("SCORE_QUIZ")]
public class ScoreQuiz
{
    [Key]
    [Column("id_score")]
    public int IdScore { get; set; }

    [Column("id_utilisateur")]
    public int IdUtilisateur { get; set; }
    public Utilisateur Utilisateur { get; set; } = null!;

    [Column("date_quiz")]
    public DateTime DateQuiz { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("niveau")]
    public DifficulteQuiz Niveau { get; set; }

    [Column("score_obtenu")]
    public int? ScoreObtenu { get; set; }

    [Required]
    [Column("nombre_questions")]
    public int NombreQuestions { get; set; }
}