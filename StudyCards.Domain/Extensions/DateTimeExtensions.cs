namespace StudyCards.Domain.Extensions;

public static class DateTimeExtensions
{
    public static bool IsSameDay(this DateTime utcDateTime, TimeZoneInfo userTimeZone)
    {
        var userLocalNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone);
        var userLocalTargetDate = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, userTimeZone);

        return userLocalNow.Date == userLocalTargetDate.Date;
    }
}
