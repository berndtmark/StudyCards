using Moq;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.UseCases.CardStudy.Get;
using StudyCards.Domain.Entities;
using Microsoft.AspNetCore.Http;
using StudyCards.Application.Enums;

namespace StudyCards.Application.Tests.UseCasesTests;

[TestClass]
public class GetCardsToStudyUseCaseTests
{
    private Mock<IDeckRepository> _deckRepositoryMock = default!;
    private Mock<ICardRepository> _cardRepositoryMock = default!;
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = default!;
    private Mock<ICardStrategyContext> _cardStrategyContextMock = default!;
    private Mock<ICardSelectionStudyFactory> _cardSelectionStudyFactoryMock = default!;
    private GetCardsToStudyUseCase _useCase = default!;

    [TestInitialize]
    public void Setup()
    {
        _deckRepositoryMock = new Mock<IDeckRepository>();
        _cardRepositoryMock = new Mock<ICardRepository>();
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _cardStrategyContextMock = new Mock<ICardStrategyContext>();
        _cardSelectionStudyFactoryMock = new Mock<ICardSelectionStudyFactory>();

        _useCase = new GetCardsToStudyUseCase(
            _deckRepositoryMock.Object,
            _cardRepositoryMock.Object,
            _httpContextAccessorMock.Object,
            _cardStrategyContextMock.Object,
            _cardSelectionStudyFactoryMock.Object
        );
    }

    [TestMethod]
    public async Task Handle_ValidRequest_ReturnsCards()
    {
        // Arrange
        var userEmail = "test@example.com";
        var deckId = Guid.NewGuid();
        var deck = new Deck { Id = deckId, DeckSettings = new DeckSettings { ReviewsPerDay = 10 } };
        var cards = new List<Card> { new(), new() };
        var strategy = Mock.Of<ICardStrategy>();

        _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
            .Returns(new System.Security.Claims.Claim("email", userEmail));
        _deckRepositoryMock.Setup(x => x.Get(deckId, userEmail))
            .ReturnsAsync(deck);
        _cardRepositoryMock.Setup(x => x.GetByDeck(deckId))
            .ReturnsAsync(cards);
        _cardSelectionStudyFactoryMock.Setup(x => x.Create(It.IsAny<CardStudyMethodology>()))
            .Returns(strategy);
        _cardStrategyContextMock.Setup(x => x.GetCards(deck.DeckSettings.ReviewsPerDay))
            .Returns(cards);

        var request = new GetCardsToStudyUseCaseRequest
        {
            DeckId = deckId,
            StudyMethodology = CardStudyMethodology.Random
        };

        // Act
        var result = await _useCase.Handle(request);

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
        var userEmail = "test@example.com";
        var deckId = Guid.NewGuid();

        _httpContextAccessorMock.Setup(x => x.HttpContext.User.FindFirst(It.IsAny<string>()))
            .Returns(new System.Security.Claims.Claim("email", userEmail));
        _deckRepositoryMock.Setup(x => x.Get(deckId, userEmail))
            .ReturnsAsync((Deck)null!);

        var request = new GetCardsToStudyUseCaseRequest
        {
            DeckId = deckId,
            StudyMethodology = CardStudyMethodology.Random
        };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentException>(async () => await _useCase.Handle(request));
    }
}

