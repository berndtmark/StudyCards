using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Models.Request;
using StudyCards.Api.Models.Response;
using StudyCards.Application.Common;
using StudyCards.Application.UseCases.CardManagement.Commands;
using StudyCards.Application.UseCases.CardManagement.Queries;

namespace StudyCards.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CardController(IMapper mapper, ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("getcards")]
    [ProducesResponseType(typeof(PagedResult<CardResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid deckId, int pageNumber, int pageSize, string? searchTerm = null)
    {
        var result = await sender.Send(new GetCardsQuery 
        { 
            DeckId = deckId, 
            PageNumber = pageNumber, 
            PageSize = pageSize,
            SearchTerm = searchTerm
        });

        var response = mapper.Map<PagedResult<CardResponse>>(result);
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

    [HttpPost]
    [Route("addcards")]
    [ProducesResponseType(typeof(AddCardsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> AddCards(AddCardsRequest request)
    {
        var result = await sender.Send(new AddCardsCommand
        {
            DeckId = request.DeckId,
            Cards = [.. request.Cards.Select(card => (card.CardFront, card.CardBack))]
        });

        var cardsAdded = mapper.Map<IList<CardResponse>>(result.CardsAdded);
        var cardsSkipped = mapper.Map<IList<CardResponse>>(result.CardsSkipped);

        return Ok(new AddCardsResponse
        {
            CardsAdded = cardsAdded,
            CardsSkipped = cardsSkipped
        });
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
