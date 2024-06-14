using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CitiesManager.Core.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    /// <summary>
    /// Generates a JWT token using the given user's information and the configuration settings.
    /// </summary>
    /// <param name="applicationUser">ApplicationUser object</param>
    /// <returns>AuthenticationResponse that includes token</returns>
    public AuthenticationResponse CreateJwtToken(ApplicationUser applicationUser)
    {
        // Create a DateTime object representing the token expiration time by adding the number of minutes specificied
        // in the configuration to the current UTC time.
        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
        
        // Create an array of Claim objects representing the user's claims, such as their ID, name, email, etc. 
        Claim[] claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()), // Subject (user id) 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // JWT unique ID 
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), // Issued at (date and time of token generation) 
            new Claim(ClaimTypes.NameIdentifier, applicationUser.Email ?? string.Empty), // Unique name identifier of the user (Email)
            new Claim(ClaimTypes.Name, applicationUser.PersonName ?? string.Empty), // Name of the user
        };

        // Create a SymmetricSecurityKey object using the key specified in the configuration
        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
        
        // Create a SigningCredentials object with the security key and the HMACSHA256 algorithm.
        SigningCredentials signingCredentials =
            new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        
        // Create a JwtSecurityToken object with the given issuer, audience, claims, expiration and signing credentials.
        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"], 
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
        string token = securityTokenHandler.WriteToken(jwtSecurityToken);

        return new AuthenticationResponse()
        {
            Token = token, 
            Email = applicationUser.Email,
            PersonName = applicationUser.PersonName,
            Expiration = expiration, 
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpirationTime = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
        };
    }

    public ClaimsPrincipal? GetPrincipalFromJwtToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            
            ValidateIssuerSigningKey = true, 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty)),
            
            ValidateLifetime = false // should be false
        };
        JwtSecurityTokenHandler securityTokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal claimsPrincipal = securityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken 
            || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return claimsPrincipal;
    }

    /// <summary>
    /// Creates a refresh token (base 64 string of random numbers)
    /// </summary>
    /// <returns>The refresh token ini base 64 string</returns>
    private static string GenerateRefreshToken()
    {
        Byte[] bytes = new byte[64];
        var randomNumberGenerator  = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}