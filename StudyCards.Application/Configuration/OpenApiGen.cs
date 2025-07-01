using System.Reflection;

namespace StudyCards.Application.Configuration;

public static class OpenApiGen
{
    public static bool IsOpenApiGeneration()
    {
        return Assembly.GetEntryAssembly()?.GetName().Name == "GetDocument.Insider";
    }
}
