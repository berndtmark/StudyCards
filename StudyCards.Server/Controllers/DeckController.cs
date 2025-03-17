using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Helpers;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardManagement.UpdateCard;
using StudyCards.Application.UseCases.DeckManagement.AddDeck;
using StudyCards.Application.UseCases.DeckManagement.GetDeck;
using StudyCards.Application.UseCases.DeckManagement.RemoveDeck;
using StudyCards.Application.UseCases.DeckManagement.UpdateDeck;
using StudyCards.Domain.Entities;
using StudyCards.Server.Models.Request;

namespace StudyCards.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeckController(IUseCaseFactory useCaseFactory, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("getdecks")]
    [ProducesResponseType(typeof(IEnumerable<Deck>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var email = httpContextAccessor.GetEmail();

        var useCase = useCaseFactory.Create<GetDeckUseCaseRequest, IEnumerable<Deck>>();
        var result = await useCase.Handle(new GetDeckUseCaseRequest { EmailAddress = email });

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("adddeck")]
    [ProducesResponseType(typeof(Deck), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(AddDeckRequest request)
    {
        var email = httpContextAccessor.GetEmail();

        var useCase = useCaseFactory.Create<AddDeckUseCaseRequest, Deck>();
        var result = await useCase.Handle(new AddDeckUseCaseRequest
        {
            EmailAddress = email,
            DeckName = request.DeckName,
            Description = request.Description,
            ReviewsPerDay = request.ReviewsPerDay,
            NewCardsPerDay = request.NewCardsPerDay,
        });

        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [Route("updatedeck")]
    [ProducesResponseType(typeof(Deck), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(UpdateDeckRequest request)
    {
        var email = httpContextAccessor.GetEmail();

        var useCase = useCaseFactory.Create<UpdateDeckUseCaseRequest, Deck>();
        var result = await useCase.Handle(new UpdateDeckUseCaseRequest
        {
            DeckId = request.DeckId,
            DeckName = request.DeckName,
            Description = request.Description,
            ReviewsPerDay = request.ReviewsPerDay,
            NewCardsPerDay = request.NewCardsPerDay,
            EmailAddress = email,
        });

        return Ok(result);
    }

    [Authorize]
    [HttpDelete]
    [Route("removedeck/{deckId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(string deckId)
    {
        var useCase = useCaseFactory.Create<RemoveDeckUseCaseRequest, bool>();
        var result = await useCase.Handle(new RemoveDeckUseCaseRequest { DeckId = new Guid(deckId) });

        return Ok(result);
    }
}
