namespace StudyCards.Application.Extensions;

public static class TimeZoneExtensions
{
    public static TimeZoneInfo GetTimeZone(this string timeZoneId)
    {
        if (string.IsNullOrEmpty(timeZoneId))
            return TimeZoneInfo.Utc;
        
        if (!TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out var userTimeZone))
        {
            userTimeZone = TimeZoneInfo.Utc;
        }

        return userTimeZone;
    }
}
