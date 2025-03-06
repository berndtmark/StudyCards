using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using StudyCards.Server.Configuration.Options;

namespace StudyCards.Server.Configuration;

public static class SecurityConfiguration
{
    public static IServiceCollection AddSecurityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/api/auth/login";
            options.LogoutPath = "/api/auth/logout";
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        })
        .AddGoogle(options =>
        {
            var googleAuthConfig = configuration.GetSection("GoogleAuth").Get<GoogleAuthOptions>() ?? throw new InvalidOperationException("Google Auth options not found.");

            options.ClientId = googleAuthConfig.ClientId;
            options.ClientSecret = googleAuthConfig.ClientSecret;
            options.CallbackPath = "/api/auth/callback";
            options.AccessDeniedPath = "/todo";
        });

        return services;
    }
}
