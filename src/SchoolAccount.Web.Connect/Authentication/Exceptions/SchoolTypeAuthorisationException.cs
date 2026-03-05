using SchoolAccount.Integration.DfESignIn.Exceptions;

namespace SchoolAccount.Web.Connect.Authentication.Exceptions;

public class SchoolTypeAuthorisationException : AuthorisationException
{
    public SchoolTypeAuthorisationException()
    {
    }
    
    public SchoolTypeAuthorisationException(string message) : base(message)
    {
    }

    public SchoolTypeAuthorisationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}