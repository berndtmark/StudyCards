using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Mapper;
using StudyCards.Api.Models.Response;
using StudyCards.Application.Extensions;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.UseCases.Statistics.Queries;

namespace StudyCards.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StatisticController(ICQRSDispatcher dispatcher, StatisticMapper statisticMapper, IHttpContextAccessor httpContextAccessor) : BaseController
{
    [HttpGet]
    [Route("studystatistics", Name = nameof(GetStudyStatistics))]
    [ProducesResponseType(typeof(StudyStatisticResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudyStatistics([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.GetUserId();

        var query = new GetStudyStatisticsQuery
        {
            UserId = new Guid(userId),
            From = from,
            To = to,
        };
        var result = await dispatcher.Send(query, cancellationToken);

        var response = statisticMapper.Map(result.Data!);
        return Ok(response);
    }
}
