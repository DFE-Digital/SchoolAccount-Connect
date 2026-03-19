using System.Security.Claims;

namespace SchoolAccount.AuthenticationTests.Helpers;

public static class ClaimsPrincipalHelper
{
    public static ClaimsPrincipal CreateUser()
    {
        return new ClaimsPrincipal(new ClaimsIdentity([new Claim("sub", "123")], "test"));
    }
}
