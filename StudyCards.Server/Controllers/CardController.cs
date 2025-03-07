using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardManagement.AddCard;
using StudyCards.Application.UseCases.CardManagement.GetCards;
using StudyCards.Application.UseCases.CardManagement.UpdateCard;
using StudyCards.Domain.Entities;
using StudyCards.Server.Models.Request;

namespace StudyCards.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController(IUseCaseFactory useCaseFactory) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("getcards")]
    public async Task<IActionResult> Get(Guid deckId)
    {
        var useCase = useCaseFactory.Create<GetCardsUseCaseRequest, IEnumerable<Card>>();
        var result = await useCase.Handle(new GetCardsUseCaseRequest { DeckId = deckId });

        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [Route("updatecard")]
    public async Task<IActionResult> Update(UpdateCardRequest request)
    {
        var useCase = useCaseFactory.Create<UpdateCardUseCaseRequest, Card>();
        var result = await useCase.Handle(new UpdateCardUseCaseRequest
        {
            CardId = request.CardId,
            CardFront = request.CardFront,
            CardBack = request.CardBack
        });

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    [Route("addcard")]
    public async Task<IActionResult> Add(AddCardRequest request)
    {
        var useCase = useCaseFactory.Create<AddCardUseCaseRequest, bool>();
        var result = await useCase.Handle(new AddCardUseCaseRequest
        {
            DeckId = request.DeckId,
            CardFront = request.CardFront,
            CardBack = request.CardBack
        });

        return Ok(result);
    }
}
