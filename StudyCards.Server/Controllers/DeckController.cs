using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Helpers;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.DeckManagement.GetDeck;
using StudyCards.Domain.Entities;

namespace StudyCards.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeckController(IUseCaseFactory useCaseFactory, IHttpContextAccessor httpContextAccessor) : ControllerBase
{
    [Authorize]
    [HttpGet]
    [Route("getdeck")]
    public async Task<IActionResult> Get()
    {
        var email = httpContextAccessor.GetEmail();

        var useCase = useCaseFactory.Create<GetDeckRequest, IEnumerable<Deck>>();
        var result = await useCase.Handle(new GetDeckRequest { EmailAddress = email });

        return Ok(result);
    }
}
