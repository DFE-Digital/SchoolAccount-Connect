using Microsoft.AspNetCore.Http;
using SchoolAccount.Application.Abstractions.Authentication;

namespace SchoolAccount.Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => "Test User"; // TODO Get from HTTP context

    // public string UserId =>
    //      _httpContextAccessor.HttpContext?.User.GetUserId()
    //     ?? throw new ApplicationException("User context is unavailable");
}
