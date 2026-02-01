using Moq;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.UseCases.CardStudy.Queries;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;

namespace StudyCardy.Application.Tests.UseCases;

[TestClass]
public class GetCardsToStudyQueryTests
{
    private Mock<IDeckRepository> _deckRepositoryMock = default!;
    private Mock<ICardRepository> _cardRepositoryMock = default!;
    private GetCardsToStudyQueryHandler _useCase = default!;

    [TestInitialize]
    public void Setup()
    {
        _deckRepositoryMock = new Mock<IDeckRepository>();
        _cardRepositoryMock = new Mock<ICardRepository>();

        _useCase = new GetCardsToStudyQueryHandler(
            _deckRepositoryMock.Object,
            _cardRepositoryMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ValidRequest_ReturnsCards()
    {
        // Arrange
        var deckId = Guid.NewGuid();
        var deck = new Deck { Id = deckId, DeckSettings = new DeckSettings { ReviewsPerDay = 10 } };
        var cards = new List<Card> { new(), new() };

        _deckRepositoryMock.Setup(x => x.Get(deckId, CancellationToken.None))
            .ReturnsAsync(deck);
        _cardRepositoryMock.Setup(x => x.GetCardsToStudy(deckId, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), CancellationToken.None))
            .ReturnsAsync(cards);

        var request = new GetCardsToStudyQuery
        {
            DeckId = deckId,
            StudyMethodology = CardStudyMethodology.Anki
        };

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(cards.Count, result.Data!.Count());
    }

    [TestMethod]
    public async Task Handle_DeckNotFound_ThrowsArgumentException()
    {
        // Arrange
        var deckId = Guid.NewGuid();

        _deckRepositoryMock.Setup(x => x.Get(deckId, CancellationToken.None))
            .ReturnsAsync((Deck)null!);

        var request = new GetCardsToStudyQuery
        {
            DeckId = deckId,
            StudyMethodology = CardStudyMethodology.Random
        };

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _useCase.Handle(request, CancellationToken.None));

    }
}

