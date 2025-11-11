using Microsoft.AspNetCore.SignalR;
using StudyCards.Api.Configuration;
using StudyCards.Api.Configuration.ExceptionHandlers;
using StudyCards.Api.Hubs;
using StudyCards.Application;
using StudyCards.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Host.ConfigureLogging();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.AddSecretsConfiguration();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddMemoryCache();

builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, EmailUserIdProvider>();

builder.Services.AddSecurityConfiguration(builder.Configuration);
builder.Services.AddMappingConfiguration();
builder.Services.AddOptionsConfiguration(builder.Configuration);

// Configure Application Layers
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureDatabaseServices(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

app.UseExceptionHandler();

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

app.MapHub<ChatHub>("/hub/chat-hub");    

app.Run();
