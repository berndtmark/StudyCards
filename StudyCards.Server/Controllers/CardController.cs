using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardManagement.Commands;
using StudyCards.Application.UseCases.CardManagement.Queries;
using StudyCards.Server.Models.Request;
using StudyCards.Server.Models.Response;

namespace StudyCards.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CardController(IMapper mapper, ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("getcards")]
    [ProducesResponseType(typeof(IEnumerable<CardResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid deckId)
    {
        var result = await sender.Send(new GetCardsQuery { DeckId = deckId });

        var response = mapper.Map<IEnumerable<CardResponse>>(result);
        return Ok(response);
    }

    [HttpPut]
    [Route("updatecard")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(UpdateCardRequest request)
    {
        var result = await sender.Send(new UpdateCardCommand
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
        var result = await sender.Send(new AddCardCommand
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
        var result = await sender.Send(new RemoveCardCommand { 
            CardId = new Guid(cardId),
            DeckId = new Guid(deckId)
        });

        return Ok(result);
    }
}
