using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Extensions;

namespace StudyCards.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IHttpContextAccessor httpContextAccessor, ILogger<AuthController> logger) : ControllerBase
{
    [HttpGet]
    [Route("login")]
    public IActionResult Login(string? returnUrl = "/")
    {
        return Challenge(
            new AuthenticationProperties { 
                RedirectUri = Url.Action(nameof(LoginCallback), new { returnUrl })
            },
            GoogleDefaults.AuthenticationScheme
        );
    }

    [HttpGet]
    [Route("callback")]
    public async Task<IActionResult> LoginCallback(string returnUrl)
    {
        var userEmail = httpContextAccessor.GetEmail();

        logger.LogInformation("User Logged In {Email}", userEmail);
        return LocalRedirect(returnUrl);
    }

    [HttpPost]
    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }

    [Authorize]
    [HttpGet]
    [Route("me")]
    [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status200OK)]
    public IActionResult Me()
    {
        if (!HttpContext?.User?.Identity?.IsAuthenticated ?? true)
        {
            return Unauthorized();
        }

        var claims = HttpContext?.User.Claims.ToDictionary(c => c.Type, c => c.Value);
        return Ok(claims);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("isloggedin")]
    public IActionResult IsLoggedIn()
    {
        if (!HttpContext?.User?.Identity?.IsAuthenticated ?? true)
        {
            return Unauthorized();
        }

        return Ok();
    }
}
