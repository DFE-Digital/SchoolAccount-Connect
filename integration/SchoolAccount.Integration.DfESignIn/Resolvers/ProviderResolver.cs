using SchoolAccount.Integration.AcademiesApi.Models;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Providers;

namespace SchoolAccount.Integration.DfESignIn.Resolvers;

public class ProviderResolver(IEnumerable<IProvider> providers) : IProviderResolver
{
    public IProvider Resolve(OrganisationClaim? organisation, AcademyOrganisation? academy, AcademyTrust? trust)
    {
        IProvider? provider = null;

        if (trust is not null)
        {
            provider = new TrustProvider();
        }

        if (academy is not null)
        {
            provider = new FreeSchoolProvider();
        }
        
        if (provider is null && organisation is not null)
        {
            provider = providers.FirstOrDefault(p => p.IsProvider(organisation));
        }

        return provider ?? NullProvider.Default;
    }
}
