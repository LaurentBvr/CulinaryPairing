using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CulinaryPairing.BLL.DTOs.Auth;
using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CulinaryPairing.BLL.Services;

public class AuthService : IAuthService
{
    private readonly CulinaryPairingDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(CulinaryPairingDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var emailExists = await _context.Utilisateurs
            .AnyAsync(u => u.Email == dto.Email);

        if (emailExists)
            throw new InvalidOperationException("Cet email est déjà utilisé.");

        var motDePasseHash = BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse);

        var utilisateur = new Utilisateur
        {
            Prenom = dto.Prenom,
            Nom = dto.Nom,
            Email = dto.Email,
            MotDePasse = motDePasseHash,
            Role = RoleUtilisateur.Utilisateur,
            PreferencesAlcool = true,
            RegimeDefaut = RegimeAlimentaire.Omnivore,
            NiveauCuisine = NiveauCuisine.Debutant,
            DateCreation = DateTime.UtcNow
        };

        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();

        return GenerateAuthResponse(utilisateur);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var utilisateur = await _context.Utilisateurs
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (utilisateur == null)
            return null;

        var motDePasseValide = BCrypt.Net.BCrypt.Verify(dto.MotDePasse, utilisateur.MotDePasse);

        if (!motDePasseValide)
            return null;

        return GenerateAuthResponse(utilisateur);
    }

    private AuthResponseDto GenerateAuthResponse(Utilisateur utilisateur)
    {
        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key manquante dans la configuration.");
        var jwtIssuer = _configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer manquant.");
        var jwtAudience = _configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience manquant.");
        var expirationHours = int.Parse(_configuration["Jwt:ExpirationHours"] ?? "24");

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, utilisateur.IdUtilisateur.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, utilisateur.Email),
            new Claim(ClaimTypes.NameIdentifier, utilisateur.IdUtilisateur.ToString()),
            new Claim(ClaimTypes.Name, utilisateur.Nom),
            new Claim("prenom", utilisateur.Prenom),
            new Claim(ClaimTypes.Role, utilisateur.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddHours(expirationHours);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponseDto
        {
            Token = tokenString,
            Expiration = expiration,
            IdUtilisateur = utilisateur.IdUtilisateur,
            Prenom = utilisateur.Prenom,
            Nom = utilisateur.Nom,
            Email = utilisateur.Email,
            Role = utilisateur.Role.ToString()
        };
    }
}