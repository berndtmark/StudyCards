using Microsoft.AspNetCore.Authentication;
using StudyCards.Application.Interfaces;
using StudyCards.Infrastructure.Secrets;
using System.Security.Claims;

namespace StudyCards.Api.Configuration.ClaimTransforms;

public class CustomClaimsTransformation(ISecretsManager secretsManager, ILogger<CustomClaimsTransformation> logger) : IClaimsTransformation
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
            logger.LogWarning("Authenticated user has no email claim.");
            return Task.FromResult(principal);
        }

        try
        {
            var admins = secretsManager.GetSecret<string[]>(Secrets.Admins) ?? [];
            if (admins.Contains(userEmail) && !principal.IsInRole(ClaimRole.Admin))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, ClaimRole.Admin));
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error transforming claims for user {UserEmail}", userEmail);
        }

        return Task.FromResult(principal);
    }
}
