using System.Security.Claims;
using CulinaryPairing.BLL.DTOs.Auth;
using CulinaryPairing.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CulinaryPairing.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var response = await _authService.RegisterAsync(dto);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _authService.LoginAsync(dto);

        if (response == null)
            return Unauthorized(new { message = "Email ou mot de passe invalide." });

        return Ok(response);
    }

    // GET /api/auth/me (protégé — nécessite un JWT valide)
   [HttpGet("me")]
[Authorize]
public IActionResult Me()
{
    var idUtilisateur = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var nom = User.FindFirst(ClaimTypes.Name)?.Value;
    var prenom = User.FindFirst("prenom")?.Value;
    var email = User.FindFirst(ClaimTypes.Email)?.Value;
    var role = User.FindFirst(ClaimTypes.Role)?.Value;
    return Ok(new
    {
        idUtilisateur,
        prenom,
        nom,
        email,
        role
    });
}
}