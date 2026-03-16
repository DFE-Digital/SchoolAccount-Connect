namespace SchoolAccount.Integration.DfESignIn.Exceptions;

public class NoProviderException : AuthorisationException
{
    public NoProviderException() { }

    public NoProviderException(string message)
        : base(message) { }

    public NoProviderException(string message, Exception innerException)
        : base(message, innerException) { }
}
