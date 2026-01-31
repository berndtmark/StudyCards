using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Mapper;
using StudyCards.Api.Models.Request;
using StudyCards.Api.Models.Response;
using StudyCards.Application.UseCases.CardStudy.Commands;
using StudyCards.Application.UseCases.CardStudy.Queries;
using StudyCards.Domain.Enums;

namespace StudyCards.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudyController(CardMapper cardMapper, ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("getcardstostudy/deck/{deckId}/methodology/{methodology}", Name = "getstudycard")]
    [ProducesResponseType(typeof(IEnumerable<CardResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid deckId, CardStudyMethodology methodology)
    {
        var result = await sender.Send(new GetCardsToStudyQuery { 
            DeckId = deckId, 
            StudyMethodology = methodology 
        });

        var response = cardMapper.Map(result);
        return Ok(response);
    }

    [HttpPut]
    [Route("reviewcards", Name = "reviewcards")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReviewedCard(ReviewCardsRequest request)
    {
        var result = await sender.Send(new ReviewCardsCommand
        {
            DeckId = request.DeckId,
            CardReviews = [.. request.Cards.Select(cr => new ReviewCardsCommand.CardReviewed
            {
                CardId = cr.CardId,
                CardDifficulty = cr.CardDifficulty,
                RepeatCount = cr.RepeatCount
            })]
        });

        var response = cardMapper.Map(result);
        return Ok(response);
    }
}
