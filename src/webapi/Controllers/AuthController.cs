using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly IConfiguration _configuration;

  public AuthController(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  [HttpPost("login")]
  public IActionResult Login([FromBody] LoginModel login)
  {
    // Aquí deberías validar el usuario y contraseña contra tu base de datos
    if (login.Username == "admin" && login.Password == "admin") // Ejemplo
    {
      var claims = new[]
      {
        new Claim(ClaimTypes.Name, login.Username)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddHours(1),
        signingCredentials: creds);

      return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    return Unauthorized();
  }
}

public class LoginModel
{
  public string Username { get; set; }
  public string Password { get; set; }
}