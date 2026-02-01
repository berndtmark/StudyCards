using StudyCards.Application.Common;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.CardManagement.Queries;

public class GetCardsQuery : IQuery<PagedResult<Card>>
{
    public Guid DeckId { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SearchTerm { get; set; }
}

public class GetCardsQueryHandler(ICardRepository cardRepository) : IQueryHandler<GetCardsQuery, PagedResult<Card>>
{
    public async Task<Result<PagedResult<Card>>> Handle(GetCardsQuery request, CancellationToken cancellationToken)
    {
        return await cardRepository.GetByDeck(request.DeckId, request.PageNumber, request.PageSize, request.SearchTerm, cancellationToken);
    }
}
