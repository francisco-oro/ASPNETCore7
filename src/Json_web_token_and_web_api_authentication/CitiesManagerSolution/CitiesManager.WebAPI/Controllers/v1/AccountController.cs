using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace CitiesManager.WebAPI.Controllers.v1;

[AllowAnonymous]
[ApiVersion("1.0")]
public class AccountController : CustomControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly RoleManager<ApplicationRole> _roleManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }
    
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
    
}