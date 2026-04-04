using StudyCards.Domain.Extensions;

namespace StudyCards.Domain.Tests.Extensions;

[TestClass]
public class DateTimeExtensionsTest
{
    [TestMethod]
    public void IsSameDay_WhenSameDay_ReturnsTrue()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;
        var timeZone = TimeZoneInfo.Utc;

        // Act
        var result = dateTime.IsSameDay(timeZone);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsSameDay_WhenNotSameDay_ReturnsFalse()
    {
        // Arrange
        var dateTime = DateTime.UtcNow.AddDays(1);
        var timeZone = TimeZoneInfo.Utc;

        // Act
        var result = dateTime.IsSameDay(timeZone);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsSameDay_SameDayInCustomTimeZone_ReturnsTrue()
    {
        // Arrange
        var timeZone = TimeZoneInfo.CreateCustomTimeZone("Custom-10", TimeSpan.FromHours(-10), "Custom", "Custom");
        var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        
        // Create a target datetime that is exactly noon of the current local day
        var targetLocal = localNow.Date.AddHours(12);
        var targetUtc = TimeZoneInfo.ConvertTimeToUtc(targetLocal, timeZone);

        // Act
        var result = targetUtc.IsSameDay(timeZone);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsSameDay_DifferentDayInCustomTimeZone_ReturnsFalse()
    {
        // Arrange
        var timeZone = TimeZoneInfo.CreateCustomTimeZone("Custom+10", TimeSpan.FromHours(10), "Custom", "Custom");
        var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        
        // Create a target datetime that is 1 AM of the next local day
        var targetLocal = localNow.Date.AddDays(1).AddHours(1);
        var targetUtc = TimeZoneInfo.ConvertTimeToUtc(targetLocal, timeZone);

        // Act
        var result = targetUtc.IsSameDay(timeZone);

        // Assert
        Assert.IsFalse(result);
    }
}
