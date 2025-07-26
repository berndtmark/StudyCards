using Moq;
using StudyCards.Application.Exceptions;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.UseCases.CardStudy.Queries;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Domain.Interfaces;

namespace StudyCard.Application.Tests.UseCasesTests;

[TestClass]
public class GetCardsToStudyQueryTests
{
    private Mock<IDeckRepository> _deckRepositoryMock = default!;
    private Mock<ICardRepository> _cardRepositoryMock = default!;
    private Mock<ICardsToStudyStrategyContext> _cardStrategyContextMock = default!;
    private Mock<ICardSelectionStudyFactory> _cardSelectionStudyFactoryMock = default!;
    private GetCardsToStudyQueryHandler _useCase = default!;

    [TestInitialize]
    public void Setup()
    {
        _deckRepositoryMock = new Mock<IDeckRepository>();
        _cardRepositoryMock = new Mock<ICardRepository>();
        _cardStrategyContextMock = new Mock<ICardsToStudyStrategyContext>();
        _cardSelectionStudyFactoryMock = new Mock<ICardSelectionStudyFactory>();

        _useCase = new GetCardsToStudyQueryHandler(
            _deckRepositoryMock.Object,
            _cardRepositoryMock.Object,
            _cardStrategyContextMock.Object,
            _cardSelectionStudyFactoryMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ValidRequest_ReturnsCards()
    {
        // Arrange
        var deckId = Guid.NewGuid();
        var deck = new Deck { Id = deckId, DeckSettings = new DeckSettings { ReviewsPerDay = 10 } };
        var cards = new List<Card> { new(), new() };
        var strategy = Mock.Of<ICardsToStudyStrategy>();

        _deckRepositoryMock.Setup(x => x.Get(deckId, CancellationToken.None))
            .ReturnsAsync(deck);
        _cardRepositoryMock.Setup(x => x.GetByDeck(deckId, CancellationToken.None))
            .ReturnsAsync(cards);
        _cardSelectionStudyFactoryMock.Setup(x => x.Create(It.IsAny<CardStudyMethodology>()))
            .Returns(strategy);
        _cardStrategyContextMock.Setup(x => x.GetStudyCards(deck.DeckSettings.ReviewsPerDay, deck.DeckSettings.NewCardsPerDay))
            .Returns(cards);

        var request = new GetCardsToStudyQuery
        {
            DeckId = deckId,
            StudyMethodology = CardStudyMethodology.Random
        };

        // Act
        var result = await _useCase.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(cards.Count, result.Count());
        _cardStrategyContextMock.Verify(x => x.SetStrategy(strategy), Times.Once);
        _cardStrategyContextMock.Verify(x => x.AddCards(cards), Times.Once);
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
        await Assert.ThrowsExceptionAsync<EntityNotFoundException>(async () => await _useCase.Handle(request, CancellationToken.None));
    }
}

