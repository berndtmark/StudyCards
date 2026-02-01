using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.Repositories;

namespace StudyCards.Application.UseCases.Admin.Queries;

public class GetAllUsersDeckUsageQuery : IQuery<IEnumerable<GetAllUsersDeckUsageResult>>
{
}

public record GetAllUsersDeckUsageResult(string EmailAddress, string DeckName, DateTime LastReview, int CardCount)
{
}

public class GetAllUsersDeckUsageQueryHandler(IDeckRepository deckRepository) : IQueryHandler<GetAllUsersDeckUsageQuery, IEnumerable<GetAllUsersDeckUsageResult>>
{
    public async Task<Result<IEnumerable<GetAllUsersDeckUsageResult>>> Handle(GetAllUsersDeckUsageQuery request, CancellationToken cancellationToken)
    {
        var response = await deckRepository.GetDecksForAllUsers(cancellationToken) ?? throw new ApplicationException("Decks cannot be acquired");

        var usage = response
            .Select(r => new GetAllUsersDeckUsageResult(r.UserEmail, r.DeckName, r.DeckReviewStatus.LastReview, r.CardCount ?? 0))
            .OrderBy(r => r.EmailAddress)
            .ThenBy(r => r.LastReview);

        return Result<IEnumerable<GetAllUsersDeckUsageResult>>.Success(usage);
    }
}
