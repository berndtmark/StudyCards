using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Configuration.ClaimTransforms;
using StudyCards.Api.Mapper;
using StudyCards.Application.Interfaces.CQRS;
using StudyCards.Application.UseCases.Admin.Queries;

namespace StudyCards.Api.Controllers;

[Authorize(Roles = ClaimRole.Admin)]
[ApiController]
[Route("api/[controller]")]
public class AdminController(ICQRSDispatcher dispatcher, AdminMapper adminMapper) : ControllerBase
{
    [HttpGet]
    [Route("getdeckusage")]
    public async Task<IActionResult> GetDeckUsage(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersDeckUsageQuery
        {
        };
        var result = await dispatcher.Send(query, cancellationToken);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route("getuserdetails")]
    public async Task<IActionResult> GetUserDetails(CancellationToken cancellationToken)
    {
        var query = new GetUserLoginDetailsQuery
        {
        };
        var response = await dispatcher.Send(query, cancellationToken);

        var result = adminMapper.Map(response.Data!);
        return Ok(result);
    }
}
