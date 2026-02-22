using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Mapper;
using StudyCards.Api.Models.Request;
using StudyCards.Api.Models.Response;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.UseCases.DeckManagement.Commands;
using StudyCards.Application.UseCases.DeckManagement.Queries;

namespace StudyCards.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DeckController(ICurrentUser currentUser, ICQRSDispatcher dispatcher, DeckMapper deckMapper) : ControllerBase
{
    [HttpGet]
    [Route("getdecks")]
    [ProducesResponseType(typeof(IEnumerable<DeckResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var email = currentUser.Email;

        var query = new GetDeckQuery
        {
            EmailAddress = email
        };
        var result = await dispatcher.Send(query, cancellationToken);

        var response = deckMapper.Map(result.Data!);
        return Ok(response);
    }

    [HttpPost]
    [Route("adddeck")]
    [ProducesResponseType(typeof(DeckResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(AddDeckRequest request, CancellationToken cancellationToken)
    {
        var email = currentUser.Email;

        var result = await dispatcher.Send(new AddDeckCommand
        {
            EmailAddress = email,
            DeckName = request.DeckName,
            Description = request.Description,
            ReviewsPerDay = request.ReviewsPerDay,
            NewCardsPerDay = request.NewCardsPerDay,
        }, cancellationToken);

        var response = deckMapper.Map(result.Data!);
        return Ok(response);
    }

    [HttpPut]
    [Route("updatedeck")]
    [ProducesResponseType(typeof(DeckResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(UpdateDeckRequest request, CancellationToken cancellationToken)
    {
        var email = currentUser.Email;

        var result = await dispatcher.Send(new UpdateDeckCommand
        {
            DeckId = request.DeckId,
            DeckName = request.DeckName,
            Description = request.Description,
            ReviewsPerDay = request.ReviewsPerDay,
            NewCardsPerDay = request.NewCardsPerDay,
            EmailAddress = email
        }, cancellationToken);

        var response = deckMapper.Map(result.Data!);
        return Ok(response);
    }

    [HttpDelete]
    [Route("removedeck/{deckId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(string deckId, CancellationToken cancellationToken)
    {
        var email = currentUser.Email;

        var result = await dispatcher.Send(new RemoveDeckCommand 
        { 
            DeckId = new Guid(deckId),
            EmailAddress = email
        }, cancellationToken);

        return Ok(result.Data);
    }
}
