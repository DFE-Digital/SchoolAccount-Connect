namespace SchoolAccount.Web.Connect.SignIn;

internal sealed class DfeSignInConfiguration
{
    public string Scope { get; init; } = null!;

    public Uri MetaDataUrl { get; init; } = null!;

    public Uri ApiServiceProxyUrl { get; init; } = null!;

    public Uri CallbackUrl { get; init; } = null!;

    public string ClientId { get; init; } = null!;

    public string ClientSecret { get; init; } = null!;

    public string CookieName { get; init; } = null!;

    public int CookieExpireTimeSpanInMinutes { get; init; }

    public bool SlidingExpiration { get; init; }

    public string AccessDeniedPath { get; init; } = null!;

    public bool GetClaimsFromUserInfoEndpoint { get; init; }

    public bool SaveTokens { get; init; }

    public IList<string> Scopes { get; init; } = null!;

    public Uri SignOutCallbackUrl { get; init; } = null!;

    public Uri SignOutRedirectUrl { get; init; } = null!;

    public bool DiscoverRolesWithPublicApi { get; init; }

    public string LoginPath { get; init; } = null!;
}