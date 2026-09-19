using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RegRoma.Models;
using Microsoft.Extensions.Options;

namespace RegRoma.Services;

public class JwtServices
{
  private readonly IConfiguration _configuration;

  public JwtService(IConfiguration configuration)
  {
    _configuration=configuration;
  }

  public string CreateToken(User user)
  {
    var key = _configuration["Jwt:Key"];//ключ
    var issuer = _configuration["Jwt:Issuer"];//кто выдает
    var audience = _configuration["Jwt:Audience"];//кому выдаем
    var expiresMinutes = int.Parse(_configuration["Jwt:ExpiresMinutes"]!); //через сколько истечет

   var claims = new List<Claim>
   {
     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
     new Claim(ClaimTypes.Email, user.Email),
     new Claim(ClaimTypes.Role, user.Role) 
   };
  }
}