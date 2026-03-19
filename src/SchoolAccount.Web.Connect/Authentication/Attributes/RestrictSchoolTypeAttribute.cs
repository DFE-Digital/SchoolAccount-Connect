using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Authentication.Filters;

namespace SchoolAccount.Web.Connect.Authentication.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class RestrictSchoolTypeAttribute : TypeFilterAttribute
{
    public RestrictSchoolTypeAttribute(params SchoolType[] allowedTypes)
        : base(typeof(SchoolTypeAuthorisationFilter))
    {
        Arguments = [allowedTypes];
        AllowedTypes = allowedTypes;
    }

    public SchoolType[] AllowedTypes { get; }
}
