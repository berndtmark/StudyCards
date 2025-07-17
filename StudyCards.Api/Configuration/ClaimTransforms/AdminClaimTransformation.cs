using Microsoft.AspNetCore.Authentication;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;
using System.Security.Claims;

namespace StudyCards.Api.Configuration.ClaimTransforms;

public class AdminClaimTransformation(ISecretsManager secretsManager) : IClaimsTransformation
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
        if (admins.Contains(userEmail) && !principal.IsInRole(ClaimRole.Admin))
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, ClaimRole.Admin));
        }

        return Task.FromResult(principal);
    }
}
