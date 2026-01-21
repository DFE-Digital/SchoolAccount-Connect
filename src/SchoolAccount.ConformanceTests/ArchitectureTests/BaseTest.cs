using System.Reflection;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Web.Connect;

namespace SchoolAccount.ConformanceTests.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(SchoolAccount.Domain.DependencyInjection).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IQuery<>).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(IDatabaseEntity).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(DependencyInjection).Assembly;
}