using System.Globalization;
using System.Security.Claims;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CitiesManager.Core.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public AuthenticationResponse CreateJwtToken(ApplicationUser applicationUser)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
        Claim[] claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()), // Subject (user id) 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique ID 
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)), // Issued at (date and time of token generation) 
            new Claim(ClaimTypes.NameIdentifier, applicationUser.Email ?? string.Empty), // Unique name identifier of the user (Email)
            new Claim(ClaimTypes.Name, applicationUser.PersonName ?? string.Empty), // Name of the user
        };
        
        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey()
    }
}