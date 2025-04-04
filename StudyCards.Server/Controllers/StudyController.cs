using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Application.Enums;
using StudyCards.Application.Interfaces;
using StudyCards.Application.UseCases.CardStudy.Get;
using StudyCards.Domain.Entities;
using StudyCards.Server.Models.Response;

namespace StudyCards.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StudyController(IUseCaseFactory useCaseFactory, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("getcardstostudy/deck/{deckId}/methodology/{methodology}")]
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
}
