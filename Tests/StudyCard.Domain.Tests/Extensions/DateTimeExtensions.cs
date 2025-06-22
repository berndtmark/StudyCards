using StudyCards.Domain.Extensions;

namespace StudyCard.Domain.Tests.Extensions;

[TestClass]
public class DateTimeExtensions
{
    [TestMethod]
    public void IsSameDay_WhenSameDay_ReturnsTrue()
    {
        // Arrange
        var dateTime = DateTime.UtcNow;

        // Act
        var result = dateTime.IsSameDay();

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsSameDay_WhenNotSameDay_ReturnsFalse()
    {
        // Arrange
        var dateTime = DateTime.UtcNow.AddDays(1);

        // Act
        var result = dateTime.IsSameDay();

        // Assert
        Assert.IsFalse(result);
    }
}
