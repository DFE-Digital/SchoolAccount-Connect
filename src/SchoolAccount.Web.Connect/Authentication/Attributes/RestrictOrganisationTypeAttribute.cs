using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Authentication.Filters;

namespace SchoolAccount.Web.Connect.Authentication.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RestrictOrganisationTypeAttribute : TypeFilterAttribute
{
    public RestrictOrganisationTypeAttribute(params Type[] allowedOrganisations)
        : base(typeof(OrganisationTypeAuthorisationFilter))
    {
        if (!allowedOrganisations.All(x => x.IsAssignableTo(typeof(IOrganisation))))
        {
            throw new ArgumentException($"All {nameof(allowedOrganisations)} types must implement {nameof(IOrganisation)}", nameof(allowedOrganisations));
        }
        
        Arguments = [allowedOrganisations];
        AllowedOrganisations = allowedOrganisations;
    }

    public Type[] AllowedOrganisations { get; }
}
