namespace SchoolAccount.Web.Connect.Middleware.Models;

public class GateResult(bool shouldRedirect, string? redirectAddress)
{
    public bool ShouldRedirect { get; } = shouldRedirect;
    public string? RedirectAddress { get; } = redirectAddress;

    public static GateResult Continue()
    {
        return new GateResult(false, null);
    }

    public static GateResult Redirect(string redirect)
    {
        ArgumentException.ThrowIfNullOrEmpty(redirect);
        return new GateResult(true, redirect);
    }
}
