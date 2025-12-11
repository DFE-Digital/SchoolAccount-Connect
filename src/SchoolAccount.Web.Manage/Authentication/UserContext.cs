using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Manage.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public string UserId =>
         httpContextAccessor.HttpContext?.User.Identity?.Name
        ?? throw new ApplicationException("User context is unavailable");
}
