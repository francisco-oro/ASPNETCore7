using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

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
    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    
    /// <summary>
    /// Handles user registration POST requests to /api/v1.0/account/register 
    /// </summary>
    /// <param name="registerDto">Register Data Transfer Object</param>
    /// <returns>A status 200 response with the user details if the registration was successful or a status
    /// 400 response with the error message if the request is not valid</returns>
    [HttpPost("register")]
    public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDto)
    {
        string errorMessage;
        // Validation
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
            return Ok(applicationUser);
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
    
    public async Task<ActionResult<ApplicationUser>> PostLogin(LoginDTO loginDto)
    {
        
    }
    
}