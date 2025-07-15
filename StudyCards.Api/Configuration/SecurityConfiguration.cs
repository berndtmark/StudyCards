using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using StudyCards.Api.Configuration.Options;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;
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
        });

        services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();

        return services;
    }

    // go under Confuiguration/ClaimsTransformation?
    public class MyClaimsTransformation(ISecretsManager secretsManager) : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity == null || !identity.IsAuthenticated)
            {
                return Task.FromResult(principal);
            }

            var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return Task.FromResult(principal);
            }

            var admins = secretsManager.GetSecret(Secrets.Test);
            if (admins.Contains(userEmail) && !principal.IsInRole("Admin"))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
            }

            return Task.FromResult(principal);
        }
    }
}
