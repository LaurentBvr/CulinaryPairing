using System.ComponentModel.DataAnnotations;
namespace CulinaryPairing.BLL.DTOs.Auth;
public class RegisterDto
{
    [Required(ErrorMessage = "Le prénom est requis.")]
    [MaxLength(100)]
    public string Prenom { get; set; } = null!;

    [Required(ErrorMessage = "Le nom est requis.")]
    [MaxLength(100)]
    public string Nom { get; set; } = null!;

    [Required(ErrorMessage = "L'email est requis.")]
    [EmailAddress(ErrorMessage = "Format d'email invalide.")]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
    public string MotDePasse { get; set; } = null!;
}