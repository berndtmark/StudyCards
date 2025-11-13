using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyCards.Api.Configuration.ClaimTransforms;
using StudyCards.Api.Models.Response;
using StudyCards.Application.UseCases.Admin.Queries;

namespace StudyCards.Api.Controllers;

[Authorize(Roles = ClaimRole.Admin)]
[ApiController]
[Route("api/[controller]")]
public class AdminController(ISender sender, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("getdeckusage")]
    public async Task<IActionResult> GetDeckUsage(CancellationToken cancellationToken)
    {
        var query = new GetAllUsersDeckUsageQuery
        {
        };
        var result = await sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Route("getuserdetails")]
    public async Task<IActionResult> GetUserDetails(CancellationToken cancellationToken)
    {
        var query = new GetUserLoginDetailsQuery
        {
        };
        var response = await sender.Send(query, cancellationToken);

        var result = mapper.Map<IList<UserDetails>>(response);
        return Ok(result);
    }
}
