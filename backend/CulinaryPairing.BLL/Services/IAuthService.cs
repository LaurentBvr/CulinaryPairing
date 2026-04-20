using CulinaryPairing.BLL.DTOs.Auth;

namespace CulinaryPairing.BLL.Services;

public interface IAuthService
{
    /// <summary>
    /// Enregistre un nouvel utilisateur. Retourne un token JWT + infos user.
    /// Lance une exception si l'email est déjà utilisé.
    /// </summary>
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

    /// <summary>
    /// Authentifie un utilisateur. Retourne un token JWT + infos user.
    /// Retourne null si email/mot de passe invalide.
    /// </summary>
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}