using System.Security.Claims;
using Microsoft.FeatureManagement.FeatureFilters;

namespace SchoolAccount.Web.Connect.Infrastructure;

public class FeatureManagementContextAccessor(IHttpContextAccessor httpContextAccessor) : ITargetingContextAccessor
{
    public ValueTask<TargetingContext> GetContextAsync()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userId = httpContext?.User?.Identity?.Name;
        var groups = new List<string>();

        if (httpContext?.User != null)
        {
            foreach (var roleClaim in httpContext.User.FindAll(ClaimTypes.Role))
            {
                groups.Add(roleClaim.Value);
            }
        }

        Console.WriteLine($"User: {userId}");
        Console.WriteLine($"Groups: {groups}");
        
        return new ValueTask<TargetingContext>(new TargetingContext
        {
            UserId = userId ?? "anonymous",
            Groups = groups
        });
    }
}