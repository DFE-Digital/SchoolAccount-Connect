using System.Security.Claims;
using System.Security.Principal;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Authentication;

internal sealed class UserContext(IHttpContextAccessor contextAccessor) : IUserContext, IIdentity
{
    public string? AuthenticationType { get; } = contextAccessor.HttpContext?.User.Identity?.AuthenticationType;
    public bool IsAuthenticated { get; } = contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    public string? Id { get; } = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    public string? GivenName { get; } = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.GivenName)?.Value;
    public string? Surname { get; } = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Surname)?.Value;
    public string? Name => $"{GivenName} {Surname}".Trim();
    public string? PreferredName { get; } = contextAccessor.HttpContext?.User.FindFirst("preferred_name")?.Value;
    public string? EmailAddress { get; } = contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

    public override string ToString()
    {
        return PreferredName 
               ?? Name 
               ?? EmailAddress 
               ?? throw new InvalidDataException();
    }
}