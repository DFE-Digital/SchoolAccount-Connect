using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Tests.Common.Factories;

namespace SchoolAccount.IntegrationTests.Data;

public static class EndpointEmulatedData
{
    /// <summary>
    /// Get all endpoints that match a "return" url style parameter.
    /// </summary>
    /// <remarks>
    /// It currently doesn't cover <c>/account/{id}/login</c> routing, could do a regex to replace with a generic
    /// value but that won't cover all situations either.
    /// </remarks>
    public static IEnumerable<object[]> GetEndpointsWithUrlParameters(
        Collection<string>? endpointArgumentNames = null,
        Collection<string>? exceptions = null
    )
    {
        endpointArgumentNames ??= ["returnUrl", "returnAddress"];
        exceptions ??= ["feedback"];

        using var factory = SchoolAccountWebApplicationFactory.Create().WithDisabledAntiforgery().Build();
        var endpointDataSource = factory.Services.GetRequiredService<EndpointDataSource>();

        foreach (var endpoint in endpointDataSource.Endpoints.OfType<RouteEndpoint>())
        {
            var url = endpoint.RoutePattern.RawText?.TrimStart('/');

            if (
                string.IsNullOrEmpty(url)
                || exceptions.Any(e => url.StartsWith(e, StringComparison.OrdinalIgnoreCase) == true)
            )
            {
                continue;
            }

            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (actionDescriptor == null)
            {
                continue;
            }

            string? matchedParameterName = null;

            foreach (var parameter in actionDescriptor.Parameters)
            {
                // This will do a direct parameter match (e.g., string returnUrl)
                if (endpointArgumentNames.Any(name => parameter.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    matchedParameterName = parameter.Name;
                    break;
                }

                // Whilst this will do a model property match (e.g., LoginViewModel with a property ReturnUrl)
                var propertyMatch = parameter
                    .ParameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(prop =>
                        endpointArgumentNames.Any(name => prop.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    );

                if (propertyMatch != null)
                {
                    matchedParameterName = propertyMatch.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(matchedParameterName))
            {
                continue;
            }

            yield return ["/" + url.TrimStart('/'), matchedParameterName];
        }
    }
}
