using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace SchoolAccount.Web.Connect.SignIn;

internal static class DfeSignInExtensions
{
    public static void AddDfeSignInAuthentication(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var configuration = configurationManager.GetRequiredSection("DfeSignIn").Get<DfeSignInConfiguration>();

        if (configuration == null)
        {
            throw new ArgumentException("DfeSignInConfig is required.");
        }

        services.AddAuthentication(sharedOptions =>
        {
            sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
            .AddOpenIdConnect(options =>
            {
                options.ClientId = configuration.ClientId;
                options.ClientSecret = configuration.ClientSecret;
                options.Authority = configuration.Scope;
                options.MetadataAddress = configuration.MetaDataUrl.OriginalString;
                options.CallbackPath = new PathString(configuration.CallbackUrl.OriginalString);
                options.SignedOutRedirectUri = new PathString(configuration.SignoutRedirectUrl.OriginalString);
                options.SignedOutCallbackPath = new PathString(configuration.SignoutCallbackUrl.OriginalString);
                options.ResponseType = OpenIdConnectResponseType.IdToken;
                options.SkipUnrecognizedRequests = true;
                options.GetClaimsFromUserInfoEndpoint = configuration.GetClaimsFromUserInfoEndpoint;
                options.SaveTokens = configuration.SaveTokens;

                options.Scope.Clear();
                foreach (string scope in configuration.Scopes)
                {
                    options.Scope.Add(scope);
                }
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = configuration.CookieName;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(configuration.CookieExpireTimeSpanInMinutes);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = configuration.SlidingExpiration;
                options.LoginPath = configuration.LoginPath;
                options.AccessDeniedPath = configuration.AccessDeniedPath;
                options.LogoutPath = configuration.SignoutRedirectUrl.OriginalString;
            });
    }
}
