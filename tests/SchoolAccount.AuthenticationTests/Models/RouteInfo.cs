namespace SchoolAccount.AuthenticationTests.Models;

public abstract record RouteInfo(string Path, string Controller, string Action)
{
    public string Endpoint => $"{Controller}.{Action}";
}

public record ProtectedRouteInfo(string Path, string Controller, string Action) : RouteInfo(Path, Controller, Action);

public record PublicRouteInfo(string Path, string Controller, string Action) : RouteInfo(Path, Controller, Action);
