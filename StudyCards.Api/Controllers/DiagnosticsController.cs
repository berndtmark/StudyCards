using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudyCards.Api.Models.Request;

namespace StudyCards.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController(ILogger<DiagnosticsController> logger) : ControllerBase
{
    [HttpPost]
    [Route("log-error", Name = nameof(LogError))]
    public IActionResult LogError([FromBody] ClientLogRequest request)
    {
        logger.LogError("{ClientErrorDetails}", request.ToString());
            
        return Ok();
    }
}
