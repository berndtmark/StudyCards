using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace StudyCards.Application.Helpers;

public static class ClaimExtensions
{
    public static string GetEmail(this IHttpContextAccessor httpContextAccessor) => httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
}
