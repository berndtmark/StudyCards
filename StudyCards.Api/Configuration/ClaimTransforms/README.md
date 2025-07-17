# Supporting Multiple IClaimsTransformation Implementations in ASP.NET Core

By default, ASP.NET Core only uses a single `IClaimsTransformation` implementation. If you want to apply multiple claim transformations (e.g., admin claims, role claims, etc.), you can use a composite pattern to chain them together.

---

## ✅ Steps to Support Multiple Claim Transformers

### 1. Create a Composite Implementation

```csharp
public class CompositeClaimsTransformation(IEnumerable<IClaimsTransformation> transformers) : IClaimsTransformation
{
    private readonly IEnumerable<IClaimsTransformation> _transformers = transformers;

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        foreach (var transformer in _transformers)
        {
            principal = await transformer.TransformAsync(principal);
        }

        return principal;
    }
}
```

### 2. Register All Implementations Individually

```csharp
services.AddTransient<IClaimsTransformation, AdminClaimTransformation>();
services.AddTransient<IClaimsTransformation, RoleClaimTransformation>();
```

### 3. Register the Composite Manually

```csharp
services.AddTransient<CompositeClaimsTransformation>();

services.AddSingleton<IClaimsTransformation>(sp =>
    sp.GetRequiredService<CompositeClaimsTransformation>());
```