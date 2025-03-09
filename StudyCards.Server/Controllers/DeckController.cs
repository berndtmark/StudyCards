using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Helpers;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.DeckManagement.AddDeck;
using StudyCards.Application.UseCases.DeckManagement.GetDeck;
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
    public async Task<IActionResult> Add(AddDeckRequest request)
    {
        var email = httpContextAccessor.GetEmail();

        var useCase = useCaseFactory.Create<AddDeckUseCaseRequest, bool>();
        var result = await useCase.Handle(new AddDeckUseCaseRequest
        {
            EmailAddress = email,
            DeckName = request.DeckName
        });

        return Ok(result);
    }
}
