namespace StudyCards.Domain.Entities;

public record User : EntityBase
{
    public string UserEmail { get; init; } = string.Empty;
    public DateTime LastLogin { get; init; }

    public User UserLogin()
    {
        return this with
        {
            LastLogin = DateTime.UtcNow
        };
    }

    public static User CreateUser(string userEmail)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserEmail = userEmail,
        };
    }
}
