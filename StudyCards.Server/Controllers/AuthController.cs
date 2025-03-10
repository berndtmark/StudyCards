using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Helpers;

namespace StudyCards.Server.Controllers;

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
        // put any code after login here. e.g. adding claims, roles, user db

        logger.LogInformation("User Logged In {Email}", httpContextAccessor.GetEmail());
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
    public IActionResult Me()
    {
        if (!HttpContext?.User?.Identity?.IsAuthenticated ?? true)
        {
            return Unauthorized();
        }

        var claims = HttpContext?.User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}
