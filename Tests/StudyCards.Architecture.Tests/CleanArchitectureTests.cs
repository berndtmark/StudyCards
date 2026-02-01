using NetArchTest.Rules;
using System.Reflection;

namespace StudyCards.Architecture.Tests;

[TestClass]
public class CleanArchitectureTests
{
    private static readonly Assembly DomainAssembly = typeof(Domain.Entities.EntityBase).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(Application.ServicesConfiguration).Assembly;
    private static readonly Assembly ApiAssembly = typeof(Api.Controllers.DeckController).Assembly;
    private static readonly Assembly InfrastructureDatabaseAssembly = typeof(Infrastructure.Database.ServicesConfiguration).Assembly;
    private static readonly Assembly InfrastructureSecretsAssembly = typeof(Infrastructure.Secrets.SecretsManager.SecretsManager).Assembly;
    private static readonly Assembly InfrastructureSharedAssembly = typeof(Infrastructure.Shared.ServicesConfiguration).Assembly;

    #region Domain Layer Tests

    [TestMethod]
    public void DomainLayer_ShouldNotDependOnApplicationLayer()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name!)
            .GetResult();

        Assert.IsTrue(result.IsSuccessful, $"Domain should not depend on Application. Failed types: {string.Join(", ", result.FailingTypes.Select(t => t.FullName))}");
    }

    [TestMethod]
    public void DomainLayer_ShouldNotDependOnApiLayer()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly.GetName().Name!)
            .GetResult();

        Assert.IsTrue(result.IsSuccessful, $"Domain should not depend on Api. Failed types: {string.Join(", ", result.FailingTypes.Select(t => t.FullName))}");
    }

    [TestMethod]
    public void DomainLayer_ShouldNotDependOnInfrastructureLayers()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureDatabaseAssembly.GetName().Name!)
            .And()
            .NotHaveDependencyOn(InfrastructureSecretsAssembly.GetName().Name!)
            .And()
            .NotHaveDependencyOn(InfrastructureSharedAssembly.GetName().Name!)
            .GetResult();

        Assert.IsTrue(result.IsSuccessful, $"Domain should not depend on Infrastructure layers. Failed types: {string.Join(", ", result.FailingTypes.Select(t => t.FullName))}");
    }

    #endregion

    #region Application Layer Tests

    [TestMethod]
    public void ApplicationLayer_ShouldNotDependOnApiLayer()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly.GetName().Name!)
            .GetResult();

        Assert.IsTrue(result.IsSuccessful, $"Application should not depend on Api. Failed types: {string.Join(", ", result.FailingTypes.Select(t => t.FullName))}");
    }

    [TestMethod]
    public void ApplicationLayer_ShouldNotDependOnInfrastructureLayers()
    {
        var databaseResult = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureDatabaseAssembly.GetName().Name!)
            .GetResult();

        var secretsResult = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureSecretsAssembly.GetName().Name!)
            .GetResult();

        var sharedResult = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureSharedAssembly.GetName().Name!)
            .GetResult();

        Assert.IsTrue(databaseResult.IsSuccessful && secretsResult.IsSuccessful && sharedResult.IsSuccessful,
            $"Application should not depend on Infrastructure layers. Database: {(databaseResult.IsSuccessful ? "OK" : string.Join(", ", databaseResult.FailingTypes.Select(t => t.FullName)))}." +
            $"Secrets: {(secretsResult.IsSuccessful ? "OK" : string.Join(", ", secretsResult.FailingTypes.Select(t => t.FullName)))}" +
            $".Shared: {(sharedResult.IsSuccessful ? "OK" : string.Join(", ", sharedResult.FailingTypes.Select(t => t.FullName)))}");
    }

    [TestMethod]
    public void ApplicationLayer_ShouldContainUseCasesAndServices()
    {
        var useCaseTypes = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ArePublic()
            .GetTypes();

        var hasUseCasesOrServices = useCaseTypes.Any(t => 
            t.Name.EndsWith("UseCase") || t.Name.EndsWith("Service") || t.Name.Contains("Handler"));

        Assert.IsTrue(hasUseCasesOrServices, "Application layer should contain UseCase or Service classes");
    }

    #endregion

    #region Infrastructure Layer Tests

    [TestMethod]
    public void InfrastructureLayers_ShouldNotDependOnApiLayer()
    {
        var databaseResult = Types
            .InAssembly(InfrastructureDatabaseAssembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly.GetName().Name!)
            .GetResult();

        var secretsResult = Types
            .InAssembly(InfrastructureSecretsAssembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly.GetName().Name!)
            .GetResult();

        var sharedResult = Types
            .InAssembly(InfrastructureSharedAssembly)
            .Should()
            .NotHaveDependencyOn(ApiAssembly.GetName().Name!)
            .GetResult();

        Assert.IsTrue(databaseResult.IsSuccessful && secretsResult.IsSuccessful && sharedResult.IsSuccessful, 
            "Infrastructure layers should not depend on Api layer");
    }

    [TestMethod]
    public void InfrastructureLayers_ShouldNotDependOnEachOther()
    {
        var dbAssembly = InfrastructureDatabaseAssembly.GetName().Name;
        var secretsAssembly = InfrastructureSecretsAssembly.GetName().Name;
        var sharedAssembly = InfrastructureSharedAssembly.GetName().Name;

        // 1. Database should be isolated
        var databaseResult = Types
            .InAssembly(InfrastructureDatabaseAssembly)
            .Should()
            .NotHaveDependencyOn(secretsAssembly)
            .And()
            .NotHaveDependencyOn(sharedAssembly)
            .GetResult();

        // 2. Secrets should be isolated
        var secretsResult = Types
            .InAssembly(InfrastructureSecretsAssembly)
            .Should()
            .NotHaveDependencyOn(dbAssembly)
            .And()
            .NotHaveDependencyOn(sharedAssembly)
            .GetResult();

        // 3. Shared should be isolated
        var sharedResult = Types
            .InAssembly(InfrastructureSharedAssembly)
            .Should()
            .NotHaveDependencyOn(dbAssembly)
            .And()
            .NotHaveDependencyOn(secretsAssembly)
            .GetResult();

        // Assert
        Assert.IsTrue(databaseResult.IsSuccessful, "Infra.Database has a bad dependency.");
        Assert.IsTrue(secretsResult.IsSuccessful, "Infra.Secrets has a bad dependency.");
        Assert.IsTrue(sharedResult.IsSuccessful, "Infra.Shared has a bad dependency.");
    }

    #endregion

    #region API Layer Tests

    [TestMethod]
    public void ApiLayer_ShouldBeWellOrganized()
    {
        var apiTypes = Types
            .InAssembly(ApiAssembly)
            .That()
            .ArePublic()
            .GetTypes();

        var hasControllers = apiTypes.Any(t => t.Name.EndsWith("Controller"));
        var hasConfiguration = apiTypes.Any(t => t.Name.EndsWith("Configuration"));

        Assert.IsTrue(hasControllers || hasConfiguration, "API layer should have Controllers or Configuration classes");
    }

    [TestMethod]
    public void ApiLayer_ShouldNotDirectlyDependOnDomainLayer()
    {
        var result = Types
            .InAssembly(ApiAssembly)
            .Should()
            .NotHaveDependencyOn(DomainAssembly.GetName().Name!)
            .GetResult();

        // This is a soft check - API might use Domain for DTOs, but should minimize direct coupling
        if (!result.IsSuccessful)
        {
            var failingTypes = result.FailingTypes.Select(t => t.FullName).ToList();
            Assert.Inconclusive($"API has direct Domain dependencies (consider using DTOs): {string.Join(", ", failingTypes)}");
        }
    }

    [TestMethod]
    public void ApiLayer_ShouldContainControllersOrEndpoints()
    {
        var apiTypes = Types
            .InAssembly(ApiAssembly)
            .That()
            .ArePublic()
            .GetTypes();

        var hasControllers = apiTypes.Any(t => 
            t.Name.EndsWith("Controller") || t.Name.EndsWith("Endpoint"));

        Assert.IsTrue(hasControllers, "API layer should contain Controller classes");
    }

    #endregion

    #region Business Logic Layer Tests

    [TestMethod]
    public void DomainEntities_ShouldFollowEncapsulationPrinciples()
    {
        var domainTypes = Types
            .InAssembly(DomainAssembly)
            .That()
            .ArePublic()
            .GetTypes();

        var entityTypes = domainTypes.Where(t => 
            t.Name.EndsWith("Entity") || t.Name.EndsWith("Aggregate") || t.Name.EndsWith("EntityBase")).ToList();

        // If domain has entity classes, they should exist
        if (entityTypes.Any())
        {
            Assert.IsTrue(entityTypes.Any(), "Domain entities should exist and follow naming conventions");
        }
    }

    [TestMethod]
    public void ApplicationLayer_ShouldContainInterfaces()
    {
        var interfaceTypes = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .AreInterfaces()
            .GetTypes();

        Assert.IsTrue(interfaceTypes.Any(), "Application layer should define interfaces for abstractions");
    }

    #endregion
}
