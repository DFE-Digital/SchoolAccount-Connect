using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Middleware.Models;

namespace SchoolAccount.Web.Connect.Middleware.Interfaces;

public interface IRequestGate
{
    int Priority { get; }
    Task<GateResult> EvaluateAsync(HttpContext context);
}
