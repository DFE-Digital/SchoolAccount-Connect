namespace SchoolAccount.Web.Connect.SignIn;

internal sealed class DfeSignInConfiguration
{
    public string Scope { get; set; } = null!;

    public Uri MetaDataUrl { get; set; } = null!;

    public Uri APIServiceProxyUrl { get; set; } = null!;

    public Uri CallbackUrl { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public string CookieName { get; set; } = null!;

    public int CookieExpireTimeSpanInMinutes { get; set; }

    public bool SlidingExpiration { get; set; }

    public string AccessDeniedPath { get; set; } = null!;

    public bool GetClaimsFromUserInfoEndpoint { get; set; }

    public bool SaveTokens { get; set; }

#pragma warning disable CA2227
    public IList<string> Scopes { get; set; } = null!;
#pragma warning restore CA2227

    public Uri SignoutCallbackUrl { get; set; } = null!;

    public Uri SignoutRedirectUrl { get; set; } = null!;

    public bool DiscoverRolesWithPublicApi { get; set; }

    public string LoginPath { get; set; } = null!;

}