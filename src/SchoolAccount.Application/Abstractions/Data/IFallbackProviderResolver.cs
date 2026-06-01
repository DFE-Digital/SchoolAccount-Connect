using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Providers;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IFallbackProviderResolver
{
    bool TryGetProvider(string? ukPrn, [MaybeNullWhen(false)] out ProviderOverrideEntity provider);
}
