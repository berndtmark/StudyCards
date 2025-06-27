using Microsoft.Extensions.Logging;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Domain.Entities;

namespace StudyCards.Application.UseCases.DeckManagement.Commands;

public class AddDeckCommand : ICommand<Deck>
{
    public string DeckName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int ReviewsPerDay { get; set; }
    public int NewCardsPerDay { get; set; }
}

public class AddDeckCommandHandler(IUnitOfWork unitOfWork, ILogger<AddDeckCommand> logger) : ICommandHandler<AddDeckCommand, Deck>
{
    public async Task<Deck> Handle(AddDeckCommand request, CancellationToken cancellationToken)
    {
        var deck = new Deck
        {
            Id = Guid.NewGuid(),
            DeckName = request.DeckName,
            Description = request.Description,
            UserEmail = request.EmailAddress,
            DeckSettings = new DeckSettings
            {
                NewCardsPerDay = request.NewCardsPerDay,
                ReviewsPerDay = request.ReviewsPerDay,
            }
        };

        try
        {
            var result = await unitOfWork.DeckRepository.Add(deck);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (Exception)
        {
            logger.LogError("Failed to add deck {DeckName}", request.DeckName);
            return default!;
        }
    }
}
