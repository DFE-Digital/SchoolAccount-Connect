using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Domain.Providers;

namespace SchoolAccount.Infrastructure.Resolvers;

public class FallbackProviderResolver : IFallbackProviderResolver
{
    private readonly Dictionary<string, ProviderOverrideEntity> _overrides;

    public FallbackProviderResolver(IEnumerable<ProviderOverrideEntity> overrides)
    {
        _overrides = overrides
            .ToDictionary(x => x.UkPrn);
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
}