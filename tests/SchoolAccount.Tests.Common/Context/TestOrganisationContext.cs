using SchoolAccount.Application.Resolvers.Interfaces;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;
using SchoolAccount.Tests.Common.Builders;
using SchoolAccount.Web.Connect.Authentication;

namespace SchoolAccount.Tests.Common.Context;

public class TestOrganisationContext : IOrganisationContext
{
    public TestOrganisationContext()
    {}
    
    public TestOrganisationContext(IProvider? provider, IOrganisation? organisation, SchoolType? type)
    {
        Provider = provider;
        Organisation = organisation;
        Type.Add(organisation?.Ukrpn ?? string.Empty, type ?? SchoolType.Unknown);
    }

    public IProvider Provider => field ?? NullProvider.Default;
    public Dictionary<string, SchoolType> Type { get; } = [];

    public IOrganisation Organisation => field ?? NullOrganisation.Default;

    public bool IsDsiDetermined { get; }

    public Task<bool> IsValid()
    {
        return Task.FromResult(true);
    }

    public async Task<bool> IsAuthorised()
    {
        return await IsValid() && Provider is not NullProvider;
    }
}