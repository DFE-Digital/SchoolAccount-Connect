using System.Diagnostics;
using SchoolAccount.Application.Abstractions.Telemetry;

namespace SchoolAccount.Web.Connect.Infrastructure;

public sealed class RequestContext(IHttpContextAccessor httpContextAccessor) : IRequestContext
{
    public string? TraceId => Activity.Current?.Id ?? httpContextAccessor.HttpContext?.TraceIdentifier;
}
