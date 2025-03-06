using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudyCards.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [HttpGet("login")]
    [Route("login")]
    public IActionResult Login(string? returnUrl = "/")
    {
        return Challenge(
            new AuthenticationProperties { RedirectUri = returnUrl },
            GoogleDefaults.AuthenticationScheme
        );
    }

    [HttpPost("logout")]
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
        if (!httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? true)
        {
            return Unauthorized();
        }

        var claims = httpContextAccessor?.HttpContext?.User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(claims);
    }
}
