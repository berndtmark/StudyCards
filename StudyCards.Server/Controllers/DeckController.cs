using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Factory;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardManagement.GetCards;
using StudyCards.Application.UseCases.DeckManagement.GetDeck;
using StudyCards.Domain.Entities;

namespace StudyCards.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeckController(IUseCaseFactory useCaseFactory) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("getdeck")]
    public async Task<IActionResult> Get(string emailAddress)
    {
        var useCase = useCaseFactory.Create<GetDeckRequest, IEnumerable<Deck>>();
        var result = await useCase.Handle(new GetDeckRequest { EmailAddress = emailAddress });

        return Ok(result);
    }
}
