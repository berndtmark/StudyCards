using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using StudyCards.Api.Configuration.ClaimTransforms;
using StudyCards.Api.Configuration.Options;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.UseCases.Admin.Commands;
using System.Security.Claims;

namespace StudyCards.Api.Configuration;

public static class SecurityConfiguration
{
    public static IServiceCollection AddSecurityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            // options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/api/auth/login";
            options.LogoutPath = "/api/auth/logout";
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
        })
        .AddGoogle(options =>
        {
            var googleAuthConfig = configuration.GetSection("GoogleAuth").Get<GoogleAuthOptions>() ?? throw new InvalidOperationException("Google Auth options not found.");

            options.ClientId = googleAuthConfig.ClientId;
            options.ClientSecret = googleAuthConfig.ClientSecret;
            options.AccessDeniedPath = "/todo";

            options.Events = new AuthEvents();
        });

        services.AddTransient<IClaimsTransformation, CustomClaimsTransformation>();

        return services;
    }
}

public class AuthEvents : OAuthEvents
{
    public override async Task CreatingTicket(OAuthCreatingTicketContext context)
    {
        var email = context.Principal?.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email)) return;

        var dispatcher = context.HttpContext.RequestServices.GetRequiredService<ICQRSDispatcher>();
        var user = await dispatcher.Send(new UserLoginCommand { UserEmail = email });

        if (context.Principal?.Identity is ClaimsIdentity identity)
        {
            identity.AddClaim(new Claim("sub", user.Data?.Id.ToString() ?? throw new ApplicationException($"User Id Missing for {email}")));
        }
    }
}