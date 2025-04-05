using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardManagement.AddCard;
using StudyCards.Application.UseCases.CardManagement.GetCards;
using StudyCards.Application.UseCases.CardManagement.RemoveCard;
using StudyCards.Application.UseCases.CardManagement.UpdateCard;
using StudyCards.Domain.Entities;
using StudyCards.Server.Models.Request;
using StudyCards.Server.Models.Response;

namespace StudyCards.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CardController(IUseCaseFactory useCaseFactory, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("getcards")]
    [ProducesResponseType(typeof(IEnumerable<CardResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid deckId)
    {
        var useCase = useCaseFactory.Create<GetCardsUseCaseRequest, IEnumerable<Card>>();
        var result = await useCase.Handle(new GetCardsUseCaseRequest { DeckId = deckId });

        var response = mapper.Map<IEnumerable<CardResponse>>(result);
        return Ok(response);
    }

    [HttpPut]
    [Route("updatecard")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(UpdateCardRequest request)
    {
        var useCase = useCaseFactory.Create<UpdateCardUseCaseRequest, Card>();
        var result = await useCase.Handle(new UpdateCardUseCaseRequest
        {
            CardId = request.CardId,
            DeckId = request.DeckId,
            CardFront = request.CardFront,
            CardBack = request.CardBack
        });

        var response = mapper.Map<CardResponse>(result);
        return Ok(response);
    }

    [HttpPost]
    [Route("addcard")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(AddCardRequest request)
    {
        var useCase = useCaseFactory.Create<AddCardUseCaseRequest, Card>();
        var result = await useCase.Handle(new AddCardUseCaseRequest
        {
            DeckId = request.DeckId,
            CardFront = request.CardFront,
            CardBack = request.CardBack
        });

        var response = mapper.Map<CardResponse>(result);
        return Ok(response);
    }

    [HttpDelete]
    [Route("removecard/deck/{deckId}/card/{cardId}", Name = "removecard")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(string deckId, string cardId)
    {
        var useCase = useCaseFactory.Create<RemoveCardUseCaseRequest, bool>();
        var result = await useCase.Handle(new RemoveCardUseCaseRequest { 
            CardId = new Guid(cardId),
            DeckId = new Guid(deckId)
        });

        return Ok(result);
    }
}
