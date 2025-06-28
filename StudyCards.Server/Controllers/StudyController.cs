using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardStudy.Get;
using StudyCards.Application.UseCases.CardStudy.Review;
using StudyCards.Domain.Entities;
using StudyCards.Domain.Enums;
using StudyCards.Server.Models.Request;
using StudyCards.Server.Models.Response;

namespace StudyCards.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudyController(IUseCaseFactory useCaseFactory, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("getcardstostudy/deck/{deckId}/methodology/{methodology}", Name = "getstudycard")]
    [ProducesResponseType(typeof(IEnumerable<CardResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(Guid deckId, CardStudyMethodology methodology)
    {
        var useCase = useCaseFactory.Create<GetCardsToStudyUseCaseRequest, IEnumerable<Card>>();
        var result = await useCase.Handle(new GetCardsToStudyUseCaseRequest { 
            DeckId = deckId, 
            StudyMethodology = methodology 
        });

        var response = mapper.Map<IEnumerable<CardResponse>>(result);
        return Ok(response);
    }

    [HttpPut]
    [Route("reviewcards", Name = "reviewcards")]
    [ProducesResponseType(typeof(CardResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> ReviewedCard(ReviewCardsRequest request)
    {
        var useCase = useCaseFactory.Create<ReviewCardsUseCaseRequest, IList<Card>>();
        var result = await useCase.Handle(new ReviewCardsUseCaseRequest
        {
            DeckId = request.DeckId,
            CardReviews = [.. request.Cards.Select(cr => new ReviewCardsUseCaseRequest.CardReviewed
            {
                CardId = cr.CardId,
                CardDifficulty = cr.CardDifficulty,
                RepeatCount = cr.RepeatCount
            })]
        });

        var response = mapper.Map<IList<CardResponseWithReviews>>(result);
        return Ok(response);
    }
}
