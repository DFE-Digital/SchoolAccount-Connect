using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;
using SchoolAccount.Kernel.Cookie;

namespace SchoolAccount.Web.Connect;

public class CookieConsentContext : ICookieConsentContext
{
    private readonly string? _cookie;
    private readonly IDataProtector _protector;

    public CookieConsentContext(
        IHttpContextAccessor contextAccessor,
        IDataProtectionProvider dataProtectionProvider)
    {
        _protector = dataProtectionProvider.CreateProtector("cookie-consent");

        if (contextAccessor.HttpContext?.Request.Cookies.TryGetValue(
                CookieConsentConstants.CookieName,
                out var cookieValue) == true)
        {
            _cookie = Unprotect(cookieValue);
        }
    }

    private string? Unprotect(string value)
    {
        try
        {
            return _protector.Unprotect(value);
        }
        catch (CryptographicException)
        {
            return null;
        }
    }

    public CookieConsentState State =>
        _cookie switch
        {
            CookieConsentConstants.IdValues.Accepted => CookieConsentState.Accepted,
            CookieConsentConstants.IdValues.Rejected => CookieConsentState.Rejected,
            null => CookieConsentState.Undeclared,
            _ => CookieConsentState.Invalid
        };

    public bool IsSet => State is CookieConsentState.Accepted or CookieConsentState.Rejected;
}