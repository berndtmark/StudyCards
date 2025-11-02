var builder = DistributedApplication.CreateBuilder(args);

var cosmos = builder.AddAzureCosmosDB("cosmos-db")
    .RunAsEmulator(emulator =>
    {
        emulator.WithLifetime(ContainerLifetime.Persistent);
        emulator.WithDataVolume();
        emulator.WithGatewayPort(8081);
    });

builder.AddProject<Projects.StudyCards_Api>("studycards-api")
    .WithReference(cosmos)
    .WaitFor(cosmos);

builder.Build().Run();
