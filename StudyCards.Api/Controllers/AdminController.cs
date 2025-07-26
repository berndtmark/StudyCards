using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Configuration.ClaimTransforms;
using StudyCards.Application.UseCases.Admin.Queries;

namespace StudyCards.Api.Controllers;

[Authorize(Roles = ClaimRole.Admin)]
[ApiController]
[Route("api/[controller]")]
public class AdminController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("getdeckusage")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersDeckUsageQuery
        {
        };
        var result = await sender.Send(query, cancellationToken);

        return Ok(result);
    }
}
