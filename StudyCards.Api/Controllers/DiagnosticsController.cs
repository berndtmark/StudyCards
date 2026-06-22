using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudyCards.Api.Models.Request;
using System.Text.RegularExpressions;

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
        var rawMessage = request?.ToString() ?? string.Empty;
        var sanitizedMessage = Regex.Replace(rawMessage, @"[\r\n\t\0\f\v]+", " ").Trim();
        logger.LogError("{ClientErrorDetails}", sanitizedMessage);
            
        return Ok();
    }
}
