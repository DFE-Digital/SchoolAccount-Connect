using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Constants;
using SchoolAccount.Application.Features.Telemetry.Commands;
using SchoolAccount.Application.Features.Telemetry.Enums;
using SchoolAccount.Application.Resolvers;
using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Authentication;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Requirements;
using SchoolAccount.Integration.DfESignIn.Resolvers;
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

        services.Scan(scan =>
            scan.FromAssembliesOf(typeof(IProvider))
                .AddClasses(classes => classes.AssignableTo<IProvider>())
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );
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
                options.MetadataAddress = configuration.MetaDataUrl.ToString();
                options.CallbackPath = configuration.CallbackUrl.ToString();
                options.SignedOutRedirectUri = configuration.SignOutRedirectUrl.ToString();
                options.SignedOutCallbackPath = configuration.SignOutCallbackUrl.ToString();
                options.ResponseType = OpenIdConnectResponseType.IdToken;
                options.SkipUnrecognizedRequests = true;
                options.GetClaimsFromUserInfoEndpoint = configuration.GetClaimsFromUserInfoEndpoint;
                options.SaveTokens = configuration.SaveTokens;
                
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                // uncomment to move organisation selection to DSI
                // options.Scope.Add("organisation");
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.MapInboundClaims = false;

                // options.Scope.Clear();
                // foreach (var scope in configuration.Scopes)
                // {
                //     options.Scope.Add(scope);
                // }

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var handler = context.HttpContext.RequestServices.GetRequiredService<
                            ICommandHandler<TrackAnalyticsTelemetryCommand>
                        >();

                        var hashedUserId = TelemetryUserIdFactory.CreateHashedUserId(context.Principal!);
                        var sessionId = AnalyticsSessionIdProvider.EnsureSessionIdClaim(context.Principal!);

                        var metricCommand = new TrackAnalyticsTelemetryCommand(
                            AnalyticsMetrics.UserLogin,
                            AnalyticsTelemetryType.Metric,
                            (AnalyticsTagNames.Outcome, "success"),
                            (AnalyticsTagNames.AuthMethod, "dfe-sign-in"),
                            (AnalyticsTagNames.Journey, "sign-in"),
                            (AnalyticsTagNames.Client, "web")
                        );

                        await handler.Handle(metricCommand, context.HttpContext.RequestAborted);

                        var eventCommand = new TrackAnalyticsTelemetryCommand(
                            AnalyticsEvents.LoginSucceeded,
                            AnalyticsTelemetryType.Event,
                            (AnalyticsTagNames.UserId, hashedUserId),
                            (AnalyticsTagNames.SessionId, sessionId),
                            (AnalyticsTagNames.AuthMethod, "dfe-sign-in"),
                            (AnalyticsTagNames.Journey, "sign-in"),
                            (AnalyticsTagNames.Client, "web")
                        );

                        await handler.Handle(eventCommand, context.HttpContext.RequestAborted);
                    },
                    OnRedirectToIdentityProviderForSignOut = async context =>
                    {
                        context.HttpContext.Session.Clear();
                        await Task.CompletedTask;
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
