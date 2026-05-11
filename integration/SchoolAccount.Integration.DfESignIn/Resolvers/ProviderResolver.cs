using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Providers;

namespace SchoolAccount.Integration.DfESignIn.Resolvers;

public class ProviderResolver(IEnumerable<IProvider> providers) : IProviderResolver
{
    public IProvider Resolve(OrganisationClaim? organisation)
    {
        IProvider? provider = null;

        if (organisation is not null)
        {
            provider = providers.FirstOrDefault(p => p.IsProvider(organisation));
        }

        return provider ?? NullProvider.Default;
    }
}
