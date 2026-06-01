namespace SchoolAccount.AuthenticationTests.Extensions;

public static class HttpResponseMessageExtensions
{
    public static bool IsAuthChallenge(this HttpResponseMessage message)
    {
        return (int)message.StatusCode is (>= 300 and < 400) or 401 or 403;
    }

    public static bool IsSuccessOrAllowed(this HttpResponseMessage message)
    {
        return (int)message.StatusCode is >= 200 and < 300 or 304;
    }
}
