using StudyCards.Application.Interfaces;
using System.Security.Claims;

namespace StudyCards.Api.Configuration.Claims;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string UserId => httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value ?? string.Empty;

    public string Email => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
}
