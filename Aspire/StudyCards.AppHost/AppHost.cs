var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
    .RunAsEmulator(emulator =>
    {
        emulator.WithLifetime(ContainerLifetime.Persistent);
        emulator.WithDataVolume();
        emulator.WithGatewayPort(8081);
    });

var studyCards = cosmos.AddCosmosDatabase("StudyCards");
var deck = studyCards.AddContainer("Deck", "/UserEmail");
var card = studyCards.AddContainer("Card", "/DeckId");
var user = studyCards.AddContainer("User", "/UserEmail");

builder.AddProject<Projects.StudyCards_Api>("studycards-api")
    .WithReference(cosmos)
    .WaitFor(cosmos);

builder.Build().Run();
