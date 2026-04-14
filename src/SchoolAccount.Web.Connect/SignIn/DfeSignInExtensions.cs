using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Telemetry.Commands;
using SchoolAccount.Application.Resolvers;
using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Authentication;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Integration.DfESignIn.Requirements;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Telemetry;

namespace SchoolAccount.Web.Connect.SignIn;

internal static class DfeSignInExtensions
{
    public static void AddDfeSignInAuthentication(
        this IServiceCollection services,
        IConfigurationManager configurationManager
    )
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
        services.AddScoped<IOrganisationResolver, OrganisationResolver>();

        services.AddScoped<ICommandHandler<TrackAnalyticsTelemetryCommand>, TrackAnalyticsTelemetryCommandHandler>();

        services
            .AddAuthentication(sharedOptions =>
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

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var handler = context.HttpContext.RequestServices.GetRequiredService<
                            ICommandHandler<TrackAnalyticsTelemetryCommand>
                        >();

                        var hashedUserId = TelemetryUserIdFactory.CreateHashedUserId(context.Principal!);

                        var command = new TrackAnalyticsTelemetryCommand(
                            AnalyticsMetrics.UserLogin,
                            new[]
                            {
                                (AnalyticsTagNames.Outcome, "success"),
                                (AnalyticsTagNames.AuthMethod, "dfe-sign-in"),
                                (AnalyticsTagNames.Journey, "sign-in"),
                                (AnalyticsTagNames.UserId, hashedUserId),
                                (AnalyticsTagNames.Scheme, context.Scheme.Name),
                            }
                        );

                        await handler.Handle(command, context.HttpContext.RequestAborted);
                    },
                };
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
            .SetDefaultPolicy(
                new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new ProviderRequirement())
                    .Build()
            );
    }
}
