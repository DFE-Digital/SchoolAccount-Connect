using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.AuthenticationTests.Helpers;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders.Shared;
using Xunit;

namespace SchoolAccount.Web.Connect.UnitTests.Builders;

public class BasicPageViewBuilderTests
{
    [Fact]
    public void Successfully_handles_an_empty_OrganisationContext()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var viewBuilder = new BasicPageViewBuilder(organisationContext);

        // Act
        var viewModel = viewBuilder.Build();

        // Assert
        viewModel.OrganisationName.Should().BeNull();
        viewModel.HasOrganisationName.Should().BeFalse();
    }

    [Fact]
    public void Build_does_not_add_the_OrganisationName_when_authentication_is_false()
    {
        // Arrange
        var schoolName = "Test School";
        var organisationContext = OrganisationContextHelper.CreateSimpleOrganisationContext(schoolName, false);
        var viewBuilder = new BasicPageViewBuilder(organisationContext);

        // Act
        var viewModel = viewBuilder.Build();

        // Assert
        viewModel.OrganisationName.Should().BeNull();
        viewModel.HasOrganisationName.Should().BeFalse();
    }

    [Fact]
    public void Build_successfully_adds_the_OrganisationName_when_authentication_is_true()
    {
        // Arrange
        var schoolName = "Test School";
        var organisationContext = OrganisationContextHelper.CreateSimpleOrganisationContext(schoolName);
        var viewBuilder = new BasicPageViewBuilder(organisationContext);

        // Act
        var viewModel = viewBuilder.Build();

        // Assert
        viewModel.OrganisationName.Should().Be(schoolName);
        viewModel.HasOrganisationName.Should().BeTrue();
    }
}
