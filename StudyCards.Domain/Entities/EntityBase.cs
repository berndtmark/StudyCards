using StudyCards.Domain.DomainEvents;

namespace StudyCards.Domain.Entities;

public abstract record EntityBase : DomainEventBase
{
    public Guid Id { get; init; }
    public DateTime CreatedDate { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; init; } = DateTime.UtcNow;
    public string CreatedBy { get; init; } = string.Empty;
    public string UpdatedBy { get; init; } = string.Empty;
}
