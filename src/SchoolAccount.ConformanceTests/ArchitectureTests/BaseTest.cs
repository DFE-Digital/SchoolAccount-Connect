using System.Reflection;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Domain.Teams;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Web.Manage;

namespace SchoolAccount.ConformanceTests.ArchitectureTests;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(Team).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IQuery<>).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(IDatabaseEntity).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(DependencyInjection).Assembly;
}