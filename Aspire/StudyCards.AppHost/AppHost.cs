var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
    .RunAsEmulator(emulator =>
    {
        emulator.WithLifetime(ContainerLifetime.Persistent);
        emulator.WithDataVolume("cosmos-data");
        emulator.WithGatewayPort(8081);
    });

var studyCards = cosmos.AddCosmosDatabase("StudyCards");
var deck = studyCards.AddContainer("Deck", "/UserId");
var card = studyCards.AddContainer("Card", "/DeckId");
var user = studyCards.AddContainer("User", "/UserEmail");
var statistic = studyCards.AddContainer("Statistic", "/UserId");

builder.AddProject<Projects.StudyCards_Api>("studycards-api")
    .WithReference(cosmos)
    .WaitFor(cosmos);

builder.Build().Run();
