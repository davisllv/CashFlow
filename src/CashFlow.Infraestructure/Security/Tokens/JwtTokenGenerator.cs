using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Infraestructure.Security.Tokens;
public class JwtTokenGenerator : IAcessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signinKey;

    // Definir o tempo de expiração do token;
    public JwtTokenGenerator(uint expirationTimeMinutes, string signKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signinKey = signKey;
    }

    public string Generate(User user)
    {
        var claims = new List<Claim>() // Claims são os valores que ficam no payload, logo, parcimônia para escolher os dados que vão ficar no payload, visto que ele é descriptografado.
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes), // Definir quando vai ser o tempo de expiração, pode ser outras coisas, dias, meses, anos, minutos, segundos
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
    
    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signinKey); // Forma de pegar os bytes de uma string;

        return new SymmetricSecurityKey(key); // SymmetricSecurityKey recebe apenas bytes
    }
}
