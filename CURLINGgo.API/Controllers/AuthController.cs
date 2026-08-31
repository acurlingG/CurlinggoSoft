using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CURLINGgo.API.DTOs;
using CurlinggoSoft.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CURLINGgo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var identityUser = await _userManager.FindByEmailAsync(model.Email)
                                ?? await _userManager.FindByNameAsync(model.Email);

            if (identityUser == null)
            {
                return Unauthorized(new { mensaje = "Credenciales incorrectas." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(identityUser, model.Password, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return Unauthorized(new { mensaje = "Cuenta bloqueada temporalmente por múltiples intentos fallidos." });

                return Unauthorized(new { mensaje = "Credenciales incorrectas." });
            }

            // Obtener datos del perfil de usuario y rol
            var usuarioDb = await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioID == identityUser.Id);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var rolPrincipal = roles.FirstOrDefault() ?? "Cliente";

            // Generar Token JWT
            var tokenString = GenerarJwtToken(identityUser, rolPrincipal, usuarioDb);

            var response = new AuthResponseDto
            {
                Token = tokenString,
                Expiracion = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"] ?? "4320")),
                Usuario = new UsuarioInfoDto
                {
                    Id = identityUser.Id,
                    Nombre = usuarioDb?.Nombre ?? identityUser.UserName ?? "Usuario",
                    Apellidos = usuarioDb?.Apellidos ?? "",
                    Email = identityUser.Email ?? "",
                    Rol = rolPrincipal
                }
            };

            return Ok(response);
        }

        private string GenerarJwtToken(IdentityUser user, string rol, Usuario? perfil)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, perfil != null ? $"{perfil.Nombre} {perfil.Apellidos}" : user.UserName ?? ""),
                new Claim(ClaimTypes.Role, rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"] ?? "4320"));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}