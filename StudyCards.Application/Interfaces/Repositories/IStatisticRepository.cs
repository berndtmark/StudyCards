using StudyCards.Domain.Entities;

namespace StudyCards.Application.Interfaces.Repositories;

public interface IStatisticRepository
{
    Task<IEnumerable<T>> Get<T>(Guid userId, DateTime dateFrom, DateTime dateTo, CancellationToken cancellationToken = default) where T : Statistic;
    Task<IEnumerable<T>> Get<T>(Guid userId, Guid deckId, CancellationToken cancellationToken = default) where T : Statistic;
    Task<Statistic> Add(Statistic statistic);
    Statistic Update(Statistic statistic);
}
