namespace CulinaryPairing.BLL.DTOs.Auth;
public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public DateTime Expiration { get; set; }
    public int IdUtilisateur { get; set; }
    public string Prenom { get; set; } = null!;
    public string Nom { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
}