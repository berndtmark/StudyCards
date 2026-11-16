using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Common;

namespace StudyCards.Application.UseCases.DeckManagement.Queries;

public class GetDeckQuery : IQuery<IEnumerable<Deck>>
{
    public Guid UserId { get; set; }
}

public class GetDeckQueryHandler(IDeckRepository deckRepository) : IQueryHandler<GetDeckQuery, IEnumerable<Deck>>
{
    public async Task<Result<IEnumerable<Deck>>> Handle(GetDeckQuery request, CancellationToken cancellationToken)
    {
        var result = await deckRepository.GetByUser(request.UserId, cancellationToken);
        return Result<IEnumerable<Deck>>.Success(result);
    }
}
