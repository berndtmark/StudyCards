using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardManagement.GetCards;
using StudyCards.Application.UseCases.CardManagement.UpdateCard;
using StudyCards.Domain.Entities;

namespace StudyCards.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CardController(IUseCaseFactory useCaseFactory) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("getcards")]
    public async Task<IActionResult> Get(string emailAddress)
    {
        var useCase = useCaseFactory.Create<GetCardsRequest, IEnumerable<Card>>();
        var result = await useCase.Handle(new GetCardsRequest { EmailAddress = emailAddress });

        return Ok(result);
    }

    [Authorize]
    [HttpPut]
    [Route("updatecard")]
    public async Task<IActionResult> Update(Models.Request.UpdateCardRequest request)
    {
        var useCase = useCaseFactory.Create<UpdateCardRequest, Card>();
        var result = await useCase.Handle(new UpdateCardRequest
        {
            CardId = request.CardId,
            CardFront = request.CardFront,
            CardBack = request.CardBack
        });

        return Ok(result);
    }
}
