using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Extensions;

namespace StudyCards.Application.UseCases.Statistics.Commands;

public class AddStudyStatistic : ICommand<StudyStatistic>
{
    public Guid UserId { get; set; }
    public Guid DeckId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CardStudyCount { get; set; }
}

public class AddStudyStatisticHandler(IUnitOfWork unitOfWork) : ICommandHandler<AddStudyStatistic, StudyStatistic>
{
    public async Task<Result<StudyStatistic>> Handle(AddStudyStatistic request, CancellationToken cancellationToken)
    {
        var statistcs = await unitOfWork.StatisticRepository.Get<StudyStatistic>(request.UserId, request.DeckId, cancellationToken);
        var todaysStatistic = statistcs.FirstOrDefault(s => s.DateRecorded.IsSameDay());

        var statistic = todaysStatistic != null ?
            todaysStatistic.Update(request.CardStudyCount) :
            StudyStatistic.Create(request.UserId, request.DeckId, request.Name, request.CardStudyCount);

        unitOfWork.StatisticRepository.Update(statistic);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return statistic;
    }
}