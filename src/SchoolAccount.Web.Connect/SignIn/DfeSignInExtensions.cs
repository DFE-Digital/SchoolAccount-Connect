using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Authentication;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Integration.DfESignIn.Requirements;
using SchoolAccount.Kernel;

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

        services.AddScoped<IProvider, NullProvider>();
        services.AddScoped<IProvider, FreeSchoolProvider>();
        services.AddScoped<IProvider, LamsProvider>();
        services.AddScoped<IProvider, PreSixteenProvider>();
        services.AddScoped<IProvider, SpecialsProvider>();
        services.AddScoped<IProviderResolver, ProviderResolver>();
        services.AddScoped<IProviderContext>(sp => sp.GetRequiredService<IOrganisationContext>());

        services.AddAuthentication(sharedOptions =>
        {
            sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
            .AddOpenIdConnect(options =>
            {
                options.ClientId = configuration.ClientId;
                options.ClientSecret = configuration.ClientSecret;
                options.Authority = configuration.Scope;
                options.MetadataAddress = configuration.MetaDataUrl.OriginalString;
                options.CallbackPath = new PathString(configuration.CallbackUrl.OriginalString);
                options.SignedOutRedirectUri = new PathString(configuration.SignOutRedirectUrl.OriginalString);
                options.SignedOutCallbackPath = new PathString(configuration.SignOutCallbackUrl.OriginalString);
                options.ResponseType = OpenIdConnectResponseType.IdToken;
                options.SkipUnrecognizedRequests = true;
                options.GetClaimsFromUserInfoEndpoint = configuration.GetClaimsFromUserInfoEndpoint;
                options.SaveTokens = configuration.SaveTokens;

                options.Scope.Clear();
                foreach (var scope in configuration.Scopes)
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
                options.LogoutPath = configuration.SignOutRedirectUrl.OriginalString;
            });
        
        services.AddScoped<IAuthorizationHandler, ProviderAuthorisationHandler>();

        services
            .AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new ProviderRequirement())
                .Build());
    }
}
