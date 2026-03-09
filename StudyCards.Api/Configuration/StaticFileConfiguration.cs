namespace StudyCards.Api.Configuration;

public static class StaticFileConfiguration
{
    public static StaticFileOptions StaticFileOptions => new()
    {
        OnPrepareResponse = ctx =>
        {
            if (ctx.File.Name.Equals("index.html", StringComparison.OrdinalIgnoreCase))
            {
                ctx.Context.Response.Headers.Append("Cache-Control", "no-cache, no-store");
                ctx.Context.Response.Headers.Append("Pragma", "no-cache");
                ctx.Context.Response.Headers.Append("Expires", "-1");
            }
        }
    };
}
