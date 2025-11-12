using Microsoft.AspNetCore.SignalR;
using StudyCards.Api.Configuration;
using StudyCards.Api.Configuration.ExceptionHandlers;
using StudyCards.Api.Hubs;
using StudyCards.Application;
using StudyCards.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// LOGGING
builder.Host.ConfigureLogging();

// EXCEPTION HANDLING
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// SECRETS (FOR CONFIGURATION)
builder.AddSecretsConfiguration();

// CONTROLLERS
builder.Services.AddControllers();

// OPENAPI
builder.Services.AddOpenApi();

// CACHE
builder.Services.AddMemoryCache();

// SIGNAL R
builder.Services.AddSignalR();
builder.Services.AddSingleton<IUserIdProvider, EmailUserIdProvider>();

// SECURITY
builder.Services.AddSecurityConfiguration(builder.Configuration);

// MAPPERS
builder.Services.AddMappingConfiguration();

// OPTIONS
builder.Services.AddOptionsConfiguration(builder.Configuration);

// BOOTSTRAP APPLICATION LAYERS
builder.Services.ConfigureApplicationServices();
builder.Services.ConfigureInfrastructureDatabaseServices(builder.Configuration);

// BUILD
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
