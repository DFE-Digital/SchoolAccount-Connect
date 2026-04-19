namespace SchoolAccount.Web.Connect.Extensions;

public static class HttpRequestExtensions
{
    public static Uri GetFullRequestUri(this HttpRequest request)
    {
        var builder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Port = request.Host.Port ?? -1,
            Path = request.Path,
            Query = request.QueryString.ToUriComponent(),
        };

        return builder.Uri;
    }
}
