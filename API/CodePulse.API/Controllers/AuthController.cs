using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Identity;
using CodePulse.API.Repositories.Interface;

namespace CodePulse.API.Controllers;

//https://localhost:xxxx/api/blog
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }

    //POST: {apibaseurl}/api/Auth/login
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        //check email
        var identityUser = await userManager.FindByEmailAsync(request.Email);

        if (identityUser is not null)
        {
            //check password
            var checkPassWordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

            if (checkPassWordResult)
            {
                var roles = await userManager.GetRolesAsync(identityUser);

                //create token and response
                var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());


                var response = new LoginResponseDto
                {
                    Email = request.Email,
                    Token = jwtToken,
                    Roles = roles.ToList()
                };

                return Ok(response);
            }
        }
        ModelState.AddModelError("", "Email or password incorrect!");

        return ValidationProblem(ModelState);
    }


    //POST: {apibaseurl}/api/Auth/register
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        // Create the IdentityUser Object
        var user = new IdentityUser
        {
            UserName = request.Email?.Trim(),
            Email = request.Email?.Trim()
        };
        //create user
        var identityResult = await userManager.CreateAsync(user, request.Password);

        if (identityResult.Succeeded)
        {
            //add role to user (reader)
            identityResult = await userManager.AddToRoleAsync(user, "Reader");

            if (identityResult.Succeeded)
            {
                return Ok();
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
        }
        else
        {
            if (identityResult.Errors.Any())
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
        }

        return ValidationProblem(ModelState);
    }
}