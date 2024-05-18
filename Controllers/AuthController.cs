using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EduSat.TestSeries.Service.Models;
using EduSat.TestSeries.Service.Models.DTOs.Auth.Request;
using EduSat.TestSeries.Service.Models.DTOs.Auth.Response;
using EduSat.TestSeries.Service.Services.Interfaces;

namespace EduSat.TestSeries.Service.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // Identity package
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtService _jwtService;

    public AuthController(UserManager<IdentityUser> userManager, IJwtService jwtService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _roleManager = roleManager;

    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDTO user)
    {
        if (ModelState.IsValid)
        {
            IdentityUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email already Registered" },
                    Success = false
                });
            }

            IdentityUser newUser = new IdentityUser()
            {
                Email = user.Email,
                UserName = user.Username,
            };

            IdentityResult? created = await _userManager.CreateAsync(newUser, user.Password);
            if (created.Succeeded)
            {

                if (!await _roleManager.RoleExistsAsync(UserRoles.Staff))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Staff));

                var staffRole = await _roleManager.FindByNameAsync(UserRoles.Staff);

                // Assign the role to the created user
                var result = await _userManager.AddToRoleAsync(newUser, staffRole.Name);
                var role = await _userManager.GetRolesAsync(newUser);
                AuthResult authResult = await _jwtService.GenerateToken(newUser, role[0]);
                //return a token
                return Ok(authResult);
            }
            else
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = created.Errors.Select(e => e.Description).ToList(),
                    Success = false
                });
            }
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> login(LoginUserDTO user)
    {
        if (ModelState.IsValid)
        {
            IdentityUser existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser == null)
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Email address is not registered." },
                    Success = false
                });
            }

            bool isUserCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);
            if (isUserCorrect)
            {
                var role = await _userManager.GetRolesAsync(existingUser);
                AuthResult authResult = await _jwtService.GenerateToken(existingUser, role[0]);
                //return a token
                return Ok(authResult);
            }
            else
            {
                return BadRequest(new RegisterResponseDTO()
                {
                    Errors = new List<string>() { "Wrong password" },
                    Success = false
                });
            }
        }

        return BadRequest(new RegisterResponseDTO()
        {
            Errors = new List<string>() { "Invalid payload" },
            Success = false
        });
    }

    [HttpPost("refreshtoken")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDTO tokenRequest)
    {
        if (ModelState.IsValid)
        {
            var verified = await _jwtService.VerifyToken(tokenRequest);
            //
            if (!verified.Success)
            {
                return BadRequest(new AuthResult()
                {
                    // Errors = new List<string> { "invalid Token" },
                    Errors = verified.Errors,
                    Success = false
                });
            }

            var tokenUser = await _userManager.FindByIdAsync(verified.UserId);

            var role = await _userManager.GetRolesAsync(tokenUser);
            AuthResult authResult = await _jwtService.GenerateToken(tokenUser, role[0]);
            //return a token
            return Ok(authResult);


        }

        return BadRequest(new AuthResult()
        {
            Errors = new List<string> { "invalid Payload" },
            Success = false
        });



    }
}
