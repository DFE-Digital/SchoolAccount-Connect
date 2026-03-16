namespace SchoolAccount.Integration.DfESignIn.Exceptions;

public class ProviderAuthorisationException : AuthorisationException
{
    public ProviderAuthorisationException() { }

    public ProviderAuthorisationException(string message)
        : base(message) { }

    public ProviderAuthorisationException(string message, Exception innerException)
        : base(message, innerException) { }
}
