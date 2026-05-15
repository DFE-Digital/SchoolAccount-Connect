using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Providers;
using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IFallbackProviderResolver
{
    bool TryGetProvider(string? ukPrn, [MaybeNullWhen(false)] out ProviderOverrideEntity provider);
}