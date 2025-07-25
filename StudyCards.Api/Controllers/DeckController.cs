﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Models.Request;
using StudyCards.Api.Models.Response;
using StudyCards.Application.Helpers;
using StudyCards.Application.UseCases.DeckManagement.Commands;
using StudyCards.Application.UseCases.DeckManagement.Queries;

namespace StudyCards.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DeckController(IHttpContextAccessor httpContextAccessor, IMapper mapper, ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("getdecks")]
    [ProducesResponseType(typeof(IEnumerable<DeckResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var email = httpContextAccessor.GetEmail();

        var query = new GetDeckQuery
        {
            EmailAddress = email
        };
        var result = await sender.Send(query, cancellationToken);

        var response = mapper.Map<IEnumerable<DeckResponse>>(result);
        return Ok(response);
    }

    [HttpPost]
    [Route("adddeck")]
    [ProducesResponseType(typeof(DeckResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Add(AddDeckRequest request, CancellationToken cancellationToken)
    {
        var email = httpContextAccessor.GetEmail();

        var result = await sender.Send(new AddDeckCommand
        {
            EmailAddress = email,
            DeckName = request.DeckName,
            Description = request.Description,
            ReviewsPerDay = request.ReviewsPerDay,
            NewCardsPerDay = request.NewCardsPerDay,
        }, cancellationToken);

        var response = mapper.Map<DeckResponse>(result);
        return Ok(response);
    }

    [HttpPut]
    [Route("updatedeck")]
    [ProducesResponseType(typeof(DeckResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(UpdateDeckRequest request, CancellationToken cancellationToken)
    {
        var email = httpContextAccessor.GetEmail();

        var result = await sender.Send(new UpdateDeckCommand
        {
            DeckId = request.DeckId,
            DeckName = request.DeckName,
            Description = request.Description,
            ReviewsPerDay = request.ReviewsPerDay,
            NewCardsPerDay = request.NewCardsPerDay,
            EmailAddress = email
        }, cancellationToken);

        var response = mapper.Map<DeckResponse>(result);
        return Ok(response);
    }

    [HttpDelete]
    [Route("removedeck/{deckId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> Remove(string deckId, CancellationToken cancellationToken)
    {
        var email = httpContextAccessor.GetEmail();

        var result = await sender.Send(new RemoveDeckCommand 
        { 
            DeckId = new Guid(deckId),
            EmailAddress = email
        }, cancellationToken);

        return Ok(result);
    }
}
