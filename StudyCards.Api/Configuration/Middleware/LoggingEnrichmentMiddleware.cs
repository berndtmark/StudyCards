using Serilog.Context;
using StudyCards.Application.Interfaces;

namespace StudyCards.Api.Configuration.Middleware;

public class LoggingEnrichmentMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, ICurrentUser currentUser)
    {
        var userId = currentUser.UserId.ToString();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            using (LogContext.PushProperty("userId", userId))
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}
