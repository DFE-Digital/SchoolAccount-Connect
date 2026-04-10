using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Cookie;

namespace SchoolAccount.Web.Connect;

public class CookieConsentContext(IHttpContextAccessor contextAccessor) : ICookieConsentContext
{
    private readonly string? _cookie =
        contextAccessor.HttpContext?.Request.Cookies.TryGetValue(
            CookieConsentConstants.CookieName,
            out var cookieValue) == true
            ? cookieValue
            : null;

    public CookieConsentState State => _cookie switch
    {
        CookieConsentConstants.IdValues.Accepted => CookieConsentState.Accepted,
        CookieConsentConstants.IdValues.Rejected => CookieConsentState.Rejected,
        null => CookieConsentState.Undeclared,
        _ => CookieConsentState.Invalid
    };

    public bool IsSet => State is CookieConsentState.Accepted or CookieConsentState.Rejected;
}