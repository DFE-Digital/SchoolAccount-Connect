using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using AwesomeAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Testing;
using SchoolAccount.AuthenticationTests.Extensions;
using SchoolAccount.AuthenticationTests.Models;
using SchoolAccount.Web.Connect;
using Xunit;

namespace SchoolAccount.AuthenticationTests;

[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase")]
[SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity")]
[SuppressMessage("Usage", "CA2234:Pass system uri objects instead of strings")]
public class RoutingAuthenticationSmokeTests : IClassFixture<RoutingTestFixture>
{
    private readonly HttpClient _client;

    public RoutingAuthenticationSmokeTests(RoutingTestFixture fixture)
    {
        _client = fixture.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Theory]
    [MemberData(nameof(GetPublicRoutes))]
    public async Task Ensure_all_public_declared_routes_actually_return_successful_status_code(
        string route,
        string label
    )
    {
        var response = await _client.GetAsync(route, TestContext.Current.CancellationToken);

        response
            .IsSuccessOrAllowed()
            .Should()
            .BeTrue(
                because: $"[{label}] Expected public route '{route}' to be accessible but got "
                    + $"HTTP {(int)response.StatusCode}."
            );
    }

    [Theory]
    [MemberData(nameof(GetProtectedRoutes))]
    public async Task Ensure_all_authorised_routes_actually_return_a_redirect_type_status_code(
        string route,
        string label
    )
    {
        var response = await _client.GetAsync(route, TestContext.Current.CancellationToken);

        response
            .IsAuthChallenge()
            .Should()
            .BeTrue(
                $"[{label}] Expected protected route '{route}' to return 3xx/401/403 but got "
                    + $"HTTP {(int)response.StatusCode}."
            );

        if (response.StatusCode is HttpStatusCode.Found or HttpStatusCode.MovedPermanently or HttpStatusCode.SeeOther)
        {
            var directingTo = response.Headers.Location?.ToString() ?? string.Empty;
            directingTo
                .Should()
                .ContainAny(
                    [RouteConstants.Start.Index, RouteConstants.Account.Login],
                    $"[{label}] Protected route '{route}' redirected to '{directingTo}' instead of the login page."
                );
        }
    }

    private static List<RouteInfo> AllRoutes
    {
        get => field ??= DiscoverAllRoutes();
    }

    private static IEnumerable<string> IgnoreList
    {
        get
        {
            yield return "/account";
            yield return "/error";
            yield return "/maintenance";
            yield return "/feedback";
        }
    }

    public static IEnumerable<object[]> GetPublicRoutes()
    {
        return AllRoutes
            .OfType<PublicRouteInfo>()
            .Where(x => !IgnoreList.Any(i => x.Path.StartsWith(i, StringComparison.InvariantCultureIgnoreCase)))
            .Select(r => new object[] { r.Path, r.Endpoint });
    }

    public static IEnumerable<object[]> GetProtectedRoutes()
    {
        return AllRoutes
            .OfType<ProtectedRouteInfo>()
            .Where(x => !IgnoreList.Any(i => x.Path.StartsWith(i, StringComparison.InvariantCultureIgnoreCase)))
            .Select(r => new object[] { r.Path, r.Action });
    }

    private static List<RouteInfo> DiscoverAllRoutes()
    {
        var routes = new List<RouteInfo>();
        var assembly = typeof(Program).Assembly;

        var controllerTypes = assembly
            .GetTypes()
            .Where(t =>
                !t.IsAbstract && (typeof(Controller).IsAssignableFrom(t) || typeof(ControllerBase).IsAssignableFrom(t))
            );

        foreach (var controller in controllerTypes)
        {
            var controllerRoute =
                controller.GetCustomAttributes<RouteAttribute>().FirstOrDefault()?.Template
                ?? BuildConventionalControllerPath(controller);

            var controllerAllowAnon = controller.GetCustomAttribute<AllowAnonymousAttribute>() is not null;
            var controllerAuthorize = controller.GetCustomAttribute<AuthorizeAttribute>() is not null;

            var actions = controller
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m =>
                    m.GetCustomAttributes<HttpGetAttribute>().Any()
                    || m.GetCustomAttribute<RouteAttribute>() is not null
                );

            foreach (var action in actions)
            {
                var actionAllowAnon = action.GetCustomAttribute<AllowAnonymousAttribute>() is not null;
                var actionAuthorize = action.GetCustomAttribute<AuthorizeAttribute>() is not null;

                var path = BuildRoutePath(controllerRoute, action);

                if (actionAllowAnon || (!actionAuthorize && controllerAllowAnon))
                {
                    routes.Add(new PublicRouteInfo(path, controller.Name, action.Name));
                }
                else
                {
                    routes.Add(new ProtectedRouteInfo(path, controller.Name, action.Name));
                }
            }
        }

        return routes;
    }

    private static string BuildConventionalControllerPath(Type controller)
    {
        return controller
            .Name.Replace("Controller", string.Empty, StringComparison.OrdinalIgnoreCase)
            .ToLowerInvariant();
    }

    private const string ConventionalTemplate = "{controller}/{action}";

    private static string BuildRoutePath(string? controllerRouteTemplate, MethodInfo action)
    {
        var httpMethodAttr = action.GetCustomAttributes<HttpMethodAttribute>().FirstOrDefault();
        var actionRouteAttr = action.GetCustomAttribute<RouteAttribute>();

        string rawTemplate;

        var hasControllerTemplate = controllerRouteTemplate is not null;
        var hasActionTemplate = actionRouteAttr?.Template is not null;
        var hasHttpMethodTemplate = httpMethodAttr?.Template is not null;

        if (!hasControllerTemplate && !hasActionTemplate && !hasHttpMethodTemplate)
        {
            // No routing attributes anywhere — use the app's conventional template
            rawTemplate = ConventionalTemplate;
        }
        else if (hasHttpMethodTemplate)
        {
            rawTemplate = httpMethodAttr!.Template!.StartsWith(RouteConstants.Root, StringComparison.InvariantCulture)
                ? httpMethodAttr.Template
                // Attribute-routed controller + verb-level template on the action
                : $"{controllerRouteTemplate}/{httpMethodAttr.Template}";
        }
        else if (hasControllerTemplate && hasActionTemplate)
        {
            rawTemplate = actionRouteAttr!.Template!.StartsWith(RouteConstants.Root, StringComparison.InvariantCulture)
                ? actionRouteAttr.Template
                // Attribute-routed controller + [Route] on the action
                : $"{controllerRouteTemplate}/{actionRouteAttr!.Template}";
        }
        else if (hasControllerTemplate)
        {
            // Attribute-routed controller, action inherits it with no override
            rawTemplate = controllerRouteTemplate!;
        }
        else
        {
            // No controller route — action carries its own route or verb template
            rawTemplate = actionRouteAttr?.Template ?? httpMethodAttr!.Template ?? string.Empty;
        }

        // ── Token replacement ────────────────────────────────────────────────────

        var controllerName = action
            .DeclaringType!.Name.Replace("Controller", "", StringComparison.OrdinalIgnoreCase)
            .ToLowerInvariant();

        var path =
            "/"
            + rawTemplate
                .Replace("[controller]", controllerName)
                .Replace("{controller}", controllerName)
                .Replace("[action]", action.Name.ToLowerInvariant())
                .Replace("{action}", action.Name.ToLowerInvariant())
                .TrimStart('/');

        // Clean up any double-slashes introduced by combining templates
        path = Regex.Replace(path, @"/{2,}", "/");

        // ── Route parameter substitution ─────────────────────────────────────────

        var parameters = action.GetParameters();

        path = Regex.Replace(
            path,
            @"\{[^}]+\}",
            m =>
            {
                var parameterName = m.Value.Trim('{', '}').Split(':').First().TrimEnd('?');
                var parameter = parameters.SingleOrDefault(p =>
                    string.Equals(p.Name, parameterName, StringComparison.OrdinalIgnoreCase)
                );

                if (parameter is null)
                {
                    return string.Empty;
                }

                return Type.GetTypeCode(parameter.ParameterType) switch
                {
                    TypeCode.Int16
                    or TypeCode.Int32
                    or TypeCode.Int64
                    or TypeCode.UInt16
                    or TypeCode.UInt32
                    or TypeCode.UInt64 => "0",
                    TypeCode.String => "test",
                    TypeCode.Object when parameter.ParameterType == typeof(Guid) => Guid.Empty.ToString(),

                    _ => string.Empty,
                };
            }
        );

        return path.TrimEnd('/');
    }
}
