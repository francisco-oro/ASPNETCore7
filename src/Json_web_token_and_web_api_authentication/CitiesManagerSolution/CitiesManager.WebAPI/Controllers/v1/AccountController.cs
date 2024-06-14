using System.Security.Claims;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1;
/// <summary>
/// Manges user authentication and registration within the application. 
/// </summary>
[AllowAnonymous]
[ApiVersion("1.0")]
public class AccountController : CustomControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly RoleManager<ApplicationRole> _roleManager;

    /// <summary>
    /// Constructor method with DI 
    /// </summary>
    /// <param name="userManager">Responsible for creating, deleting and modifying existing users</param>
    /// <param name="signInManager">Manages the user sign-in and sign-out of in the application</param>
    /// <param name="roleManager">Manages rol-based authentication and authorization</param>
    /// <param name="jwtService">Manages JWT tokens</param>

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
    }
    private readonly IJwtService _jwtService;


    /// <summary>
    /// Handles user registration POST requests to /api/v1.0/account/register 
    /// </summary>
    /// <param name="registerDto">Register Data Transfer Object</param>
    /// <returns>A status 200 response with the user details if the registration was successful or a status
    /// 400 response with the error message if the request is not valid</returns>
    [HttpPost("register")]
    public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDto)
    {
        // string errorMessage;
        // Validation
        string errorMessage;
        if (ModelState.IsValid == false)
        {
            errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
            return Problem(errorMessage);
        }
        
        // Create user
        ApplicationUser applicationUser = new ApplicationUser()
        {
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            UserName = registerDto.Email,
            PersonName = registerDto.PersonName
        };

        IdentityResult identityResult = await _userManager.CreateAsync(applicationUser, registerDto.Password);

        if (identityResult.Succeeded)
        {
            // Sign-in
            await _signInManager.SignInAsync(applicationUser, isPersistent: false);
            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(applicationUser);
            applicationUser.RefreshToken = authenticationResponse.RefreshToken;
            applicationUser.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationTime;
            await _userManager.UpdateAsync(applicationUser);
            
            return Ok(authenticationResponse);
        }

        errorMessage = string.Join(" | ", identityResult.Errors.Select(e => e.Description));
        // error1 | error 2
        return Problem(errorMessage);
    }

    /// <summary>
    /// Handles duplicate-email verification GET requests to /api/v1.0/account/IsEmailAlreadyRegistered 
    /// </summary>
    /// <param name="email">email address in string format</param>
    /// <returns>A status 200 response with true value if the user doesn't exist and false if the user already exists in the database </returns>
    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(email);
        if (applicationUser == null)
        {
            return Ok(true);
        }

        return Ok(false);
    }
    
    /// <summary>
    /// Handles login POST requests to /api/v1.0/account/login
    /// </summary>
    /// <param name="loginDto">Login Data Transfer Object</param>
    /// <returns>A status 200 response with the user details if the login was successful or a status
    /// 400 response with the error message if the request is not valid</returns>
    [HttpPost("login")]
    public async Task<IActionResult> PostLogin(LoginDTO loginDto)
    {
        // Validation
        if (ModelState.IsValid == false)
        {
            string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(error => error.ErrorMessage));
            return Problem(errorMessage);
        }

        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return NoContent();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(user);
            user.RefreshToken = authenticationResponse.RefreshToken;
            user.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationTime;
            await _userManager.UpdateAsync(user);
            return Ok(authenticationResponse);
        }

        return Problem("Invalid email or password");
    }

    /// <summary>
    /// Handles logout GET requests to /api/v1.0/account/login
    /// </summary>
    /// <returns>A no content status 204 respose</returns>
    [HttpGet("logout")]
    public async Task<IActionResult> GetLogout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }

    /// <summary>
    /// Handles refresh-token POST requests to /api/v1.0/account/generate-new-token
    /// </summary>
    /// <param name="tokenModel">Token model class</param>
    /// <returns>Status 200 response with the authentication response class if the request is successful
    /// Status 400 error response if token is invalid or has not expired yet</returns>
    [HttpPost("generate-new-token")]
    public  async Task<IActionResult> GenerateNewAccessToken(TokenModel? tokenModel)
    {
        if (tokenModel == null)
        {
            return BadRequest("Invalid client request"); 
        }

        ClaimsPrincipal? claimsPrincipal = _jwtService.GetPrincipalFromJwtToken(tokenModel.Token);
        if (claimsPrincipal == null)
        {
            return BadRequest("Invalid jwt access token");
        }

        string? email = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        ApplicationUser? applicationUser = await _userManager.FindByEmailAsync(email);
        if (applicationUser == null 
            || applicationUser.RefreshToken != tokenModel.RefreshToken
            || applicationUser.RefreshTokenExpirationDateTime <= DateTime.Now)
        {
            return BadRequest("Invalid refresh token");
        }

        AuthenticationResponse authenticationResponse = _jwtService.CreateJwtToken(applicationUser);

        applicationUser.RefreshToken = authenticationResponse.RefreshToken;
        applicationUser.RefreshTokenExpirationDateTime = authenticationResponse.RefreshTokenExpirationTime;

        await _userManager.UpdateAsync(applicationUser);

        return Ok(authenticationResponse);
    }
    
}