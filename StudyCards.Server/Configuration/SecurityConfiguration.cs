using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
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
            // options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie(options =>
        {
            options.LoginPath = "/api/auth/login";
            options.LogoutPath = "/api/auth/logout";
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.HttpOnly = true;
            options.Events = new CookieAuthenticationEvents()
            {
                OnRedirectToLogin = (context) =>
                {
                    context.HttpContext.Response.Redirect(context.RedirectUri.Replace("http://", "https://"));
                    return Task.CompletedTask;
                }
            };
        })
        .AddGoogle(options =>
        {
            var googleAuthConfig = configuration.GetSection("GoogleAuth").Get<GoogleAuthOptions>() ?? throw new InvalidOperationException("Google Auth options not found.");

            options.ClientId = googleAuthConfig.ClientId;
            options.ClientSecret = googleAuthConfig.ClientSecret;
            options.AccessDeniedPath = "/todo";
        });

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        return services;
    }
}
