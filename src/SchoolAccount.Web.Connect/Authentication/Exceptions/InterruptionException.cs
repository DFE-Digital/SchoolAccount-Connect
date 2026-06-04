namespace SchoolAccount.Web.Connect.Authentication.Exceptions;

public class InterruptionException : Exception
{
    public InterruptionException()
    {
    }
    
    public InterruptionException(string message) : base(message)
    {
    }

    public InterruptionException(string message, Exception innerException) : base(message, innerException)
    {
    }
}