using System.Security;

namespace SchoolAccount.Integration.DfESignIn.Exceptions;

public class AuthorisationException : SecurityException
{
    protected AuthorisationException()
    {
    }

    protected AuthorisationException(string message) : base(message)
    {
    }

    protected AuthorisationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}