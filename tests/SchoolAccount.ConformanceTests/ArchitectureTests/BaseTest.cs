using System.Reflection;
using SchoolAccount.Web.Connect;

namespace SchoolAccount.ConformanceTests.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Domain.DependencyInjection).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.DependencyInjection).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(DependencyInjection).Assembly;
}
