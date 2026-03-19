using AwesomeAssertions;
using NetArchTest.Rules;
using Xunit;

namespace SchoolAccount.ConformanceTests.ArchitectureTests;

public class LayerTests : BaseTest
{
    [Fact]
    [Trait("ArchitectureTests", "LayerTests")]
    public void DomainLayerShouldNotHaveADependencyOnApplicationLayer()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Domain layer should not have a dependency on the application layer.");
    }

    [Fact]
    [Trait("ArchitectureTests", "LayerTests")]
    public void DomainLayerShouldNotHaveADependencyOnInfrastructureLayer()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Domain layer should not have a dependency on the infrastructure layer.");
    }

    [Fact]
    [Trait("ArchitectureTests", "LayerTests")]
    public void DomainLayerShouldNotHaveADependencyOnPresentationLayer()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result.IsSuccessful.Should().BeTrue("Domain layer should not have a dependency on the presentation layer.");
    }

    [Fact]
    [Trait("ArchitectureTests", "LayerTests")]
    public void ApplicationLayerShouldNotHaveADependencyOnInfrastructureLayer()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue("Application layer should not have a dependency on the infrastructure layer.");
    }

    [Fact]
    [Trait("ArchitectureTests", "LayerTests")]
    public void ApplicationLayerShouldNotHaveADependencyOnPresentationLayer()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue("Application layer should not have a dependency on the presentation layer.");
    }

    [Fact]
    [Trait("ArchitectureTests", "LayerTests")]
    public void InfrastructureLayerShouldNotHaveADependencyOnPresentationLayer()
    {
        var result = Types
            .InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
            .GetResult();

        result
            .IsSuccessful.Should()
            .BeTrue("Infrastructure layer should not have a dependency on the presentation layer.");
    }
}
