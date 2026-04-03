using StudyCards.Application.Interfaces;
using System.Security.Claims;

namespace StudyCards.Api.Configuration.Claims;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid UserId =>
        Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value, out var guid)
            ? guid
            : Guid.Empty;

    public string Email => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

    private string TimeZoneId => httpContextAccessor.HttpContext?.Request.Headers["X-Timezone"].FirstOrDefault() ?? "UTC";
    public TimeZoneInfo TimeZone => field ??= GetTimeZone();

    private TimeZoneInfo GetTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(TimeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.Utc;
        }
    }
}
