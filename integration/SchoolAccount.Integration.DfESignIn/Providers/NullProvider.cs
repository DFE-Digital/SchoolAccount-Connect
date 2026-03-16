using SchoolAccount.Integration.DfESignIn.Interfaces;

namespace SchoolAccount.Integration.DfESignIn.Providers;

public class NullProvider : IProvider
{
    public bool IsProvider(OrganisationClaim organisation)
    {
        return false;
    }

    public Task<bool> CanAccess()
    {
        return Task.FromResult(false);
    }

    public static readonly NullProvider Default = new();
}
