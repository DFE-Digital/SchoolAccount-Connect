namespace SchoolAccount.Kernel.Cookie;

public interface ICookieConsentContext
{
    CookieConsentState State { get; }
    bool IsSet { get; }
}
