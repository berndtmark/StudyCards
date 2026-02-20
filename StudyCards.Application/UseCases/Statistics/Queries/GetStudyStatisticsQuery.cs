using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.Statistics.Queries;

public class GetStudyStatisticsQuery : IQuery<IEnumerable<StudyStatistic>>
{
    public Guid UserId { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class GetStudyStatisticsQueryHandler(IStatisticRepository statisticRepository) : IQueryHandler<GetStudyStatisticsQuery, IEnumerable<StudyStatistic>>
{
    public async Task<Result<IEnumerable<StudyStatistic>>> Handle(GetStudyStatisticsQuery query, CancellationToken cancellationToken)
    {
        var statistics = await statisticRepository.Get<StudyStatistic>(query.UserId, query.From, query.To, cancellationToken);
        return Result<IEnumerable<StudyStatistic>>.Success(statistics);
    }
}
