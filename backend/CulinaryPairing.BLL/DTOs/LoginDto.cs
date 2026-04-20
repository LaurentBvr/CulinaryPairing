using System.ComponentModel.DataAnnotations;

namespace CulinaryPairing.BLL.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "L'email est requis.")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Le mot de passe est requis.")]
    public string MotDePasse { get; set; } = null!;
}