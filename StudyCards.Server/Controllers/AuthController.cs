using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace StudyCards.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
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

    //[HttpGet("callback")]
    //[Route("callback")]
    //public async Task<IActionResult> GoogleCallback()
    //{
    //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    //    if (!result.Succeeded)
    //        return Unauthorized();

    //    var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;
    //    var name = result.Principal?.FindFirst(ClaimTypes.Name)?.Value;
    //    var picture = result.Principal?.FindFirst("picture")?.Value;

    //    if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
    //        return Unauthorized();

    //    //var user = await _userRepository.UpsertUser(new User
    //    //{
    //    //    Email = email,
    //    //    Name = name,
    //    //    Picture = picture
    //    //});

    //    return LocalRedirect("/");
    //}

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}
