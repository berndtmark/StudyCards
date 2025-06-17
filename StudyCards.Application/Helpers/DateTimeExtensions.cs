namespace StudyCards.Application.Helpers;

public static class DateTimeExtensions
{
    public static bool IsSameDay(this DateTime dateTime)
    {
        return dateTime.Date == DateTime.UtcNow.Date;
    }
}
