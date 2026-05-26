using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Domain.Providers;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Sources;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Tags;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Domain.Taxonomies;
using SchoolAccount.Domain.Types;
using TaxonomyEntity = SchoolAccount.Domain.Taxonomies.TaxonomyEntity;

namespace SchoolAccount.Infrastructure;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options),
        IApplicationDbContext
{
    public DbSet<TaskEntity> Tasks { get; set; }

    public DbSet<SubTaskEntity> SubTasks { get; set; }

    public DbSet<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; set; }

    public DbSet<TagsSourceMappingEntity> TagsSourceMappings { get; set; }

    public DbSet<SchoolTypeEntity> SchoolTypes { get; set; }

    public DbSet<SourceEntity> Sources { get; set; }

    public DbSet<TagEntity> Tags { get; set; }

    public DbSet<TaxonomyEntity> Taxonomies { get; set; }

    public DbSet<TaxonomyGroupingEntity> TaxonomyGroupings { get; set; }

    public DbSet<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; set; }

    public DbSet<TypeEntity> Types { get; set; }

    public DbSet<TypeGroupingEntity> TypeGroupings { get; set; }

    public DbSet<TypeTaskMappingEntity> TypeTaskMappings { get; set; }
    
    public DbSet<ProviderOverrideEntity> ProviderOverrides { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
