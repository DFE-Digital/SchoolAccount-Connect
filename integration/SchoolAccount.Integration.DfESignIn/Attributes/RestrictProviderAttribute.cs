using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Integration.DfESignIn.Filters;

namespace SchoolAccount.Integration.DfESignIn.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RestrictProviderAttribute : TypeFilterAttribute
{
    public RestrictProviderAttribute(params Type[] allowedProviders)
        : base(typeof(ProviderAuthorisationFilter))
    {
        Arguments = [allowedProviders];
        AllowedProviders = allowedProviders;
    }

    public Type[] AllowedProviders { get; }
}