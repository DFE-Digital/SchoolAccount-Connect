using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace SchoolAccount.IntegrationTests.Features.Authentication.Handlers;

[SuppressMessage("Naming", "CA1725:Parameter names should match base declaration")]
public class DisabledAntiforgery : IAntiforgery
{
    public AntiforgeryTokenSet GetAndStoreTokens(HttpContext context)
    {
        return new AntiforgeryTokenSet("test-token", "test-token", "test-field", "test-header");
    }

    public AntiforgeryTokenSet GetTokens(HttpContext context)
    {
        return new AntiforgeryTokenSet("test-token", "test-token", "test-field", "test-header");
    }

    public Task<bool> IsRequestValidAsync(HttpContext context)
    {
        return Task.FromResult(true);
    }

    public void SetCookieTokenAndHeader(HttpContext context) { }

    public Task ValidateRequestAsync(HttpContext context)
    {
        return Task.CompletedTask;
    }
}
