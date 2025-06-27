using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Domain.Entities;
using StudyCards.Application.Interfaces.CQRS;

namespace StudyCards.Application.UseCases.DeckManagement.Queries;

public class GetDeckQuery : IQuery<IEnumerable<Deck>>
{
    public string EmailAddress { get; set; } = string.Empty;
}

public class GetDeckQueryHandler(IDeckRepository deckRepository) : IQueryHandler<GetDeckQuery, IEnumerable<Deck>>
{
    public async Task<IEnumerable<Deck>> Handle(GetDeckQuery request, CancellationToken cancellationToken)
    {
        return await deckRepository.GetByEmail(request.EmailAddress, cancellationToken);
    }
}
