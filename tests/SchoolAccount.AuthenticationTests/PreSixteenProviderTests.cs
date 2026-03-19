using AwesomeAssertions;
using SchoolAccount.Integration.DfESignIn;
using SchoolAccount.Integration.DfESignIn.Providers;
using Xunit;

namespace SchoolAccount.AuthenticationTests;

public class PreSixteenProviderTests
{
    [Fact]
    public void IsProviderShouldReturnTrueForPreSixteenTypes()
    {
        var provider = new PreSixteenProvider();
        var claim = new OrganisationClaim
        {
            Type = new OrganisationEstablishmentTypeClaim { Id = EstablishmentType.AcademyConverter },
        };

        provider.IsProvider(claim).Should().BeTrue();
    }

    [Fact]
    public void IsProviderShouldReturnFalseIfNotForPreSixteenTypes()
    {
        var provider = new PreSixteenProvider();
        var claim = new OrganisationClaim
        {
            Type = new OrganisationEstablishmentTypeClaim { Id = EstablishmentType.FreeSchools },
        };

        provider.IsProvider(claim).Should().BeFalse();
    }
}
