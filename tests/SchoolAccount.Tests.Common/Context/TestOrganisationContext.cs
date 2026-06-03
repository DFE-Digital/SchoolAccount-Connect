using SchoolAccount.Integration.DfESignIn.Interfaces;
using SchoolAccount.Integration.DfESignIn.Providers;
using SchoolAccount.Kernel;
using SchoolAccount.Kernel.Organisations;

namespace SchoolAccount.Tests.Common.Context;

public class TestOrganisationContext : IOrganisationContext
{
    private readonly IProvider? _provider;
    private readonly SchoolType? _schoolType;
    private readonly IOrganisation? _organisation;
    
    public TestOrganisationContext()
    {}
    
    public TestOrganisationContext(IProvider? provider, IOrganisation? organisation, SchoolType? type)
    {
        _provider = provider;
        _organisation = organisation;
        _schoolType = type;
    }

    public IProvider Provider => _provider ?? NullProvider.Default;
    public SchoolType Type => _schoolType ?? SchoolType.Unknown;
    public IOrganisation Organisation => _organisation ?? NullOrganisation.Default;
    
    public Task<bool> IsValid()
    {
        return Task.FromResult(true);
    }

    public async Task<bool> IsAuthorised()
    {
        return await IsValid() && Provider is not NullProvider;
    }
}