using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Domain.Providers;

namespace SchoolAccount.IntegrationTests.Features.Database.Resolvers;

public class StubFallbackProviderResolver : IFallbackProviderResolver
{
    private readonly Dictionary<string, ProviderOverrideEntity> _overrides;

    public StubFallbackProviderResolver()
    {
        _overrides = [];
    }
    
    public StubFallbackProviderResolver(Dictionary<string, ProviderOverrideEntity> overrides)
    {
        _overrides = overrides;
    }

    public StubFallbackProviderResolver(IEnumerable<ProviderOverrideEntity> overrides)
    {
        _overrides = overrides.ToDictionary(o => o.UkPrn);
    }
    
    public bool TryGetProvider(string? ukPrn, [MaybeNullWhen(false)] out ProviderOverrideEntity provider)
    {
        provider = null;
        
        if (string.IsNullOrEmpty(ukPrn))
        {
            return false;
        }
        
        return !string.IsNullOrEmpty(ukPrn) && _overrides.TryGetValue(ukPrn, out provider);
    }

    public void ClearProviders()
    {
        _overrides.Clear();
    }

    public void AddProvider(string ukPrn, ProviderOverrideEntity provider)
    {
        _overrides.Add(ukPrn, provider);
    }
}