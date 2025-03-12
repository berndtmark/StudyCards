using StudyCards.Application;
using StudyCards.Infrastructure.Database;
using StudyCards.Server.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

builder.Services.AddSecretsConfiguration(builder.Configuration);
builder.Services.AddSecurityConfiguration(builder.Configuration);

builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.ConfigureInfrastructureDatabaseServices(builder.Configuration);

var app = builder.Build();

// Configure static files and default files
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Disable caching for all static files to ensure updates are immediately available
        ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store";
        ctx.Context.Response.Headers["Pragma"] = "no-cache";
        ctx.Context.Response.Headers["Expires"] = "-1";
    }
});
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // await app.Services.CreateDatabaseForLocal(app.Configuration);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
