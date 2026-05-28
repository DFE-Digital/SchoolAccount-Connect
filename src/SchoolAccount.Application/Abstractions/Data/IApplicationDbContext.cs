using Microsoft.EntityFrameworkCore;
using SchoolAccount.Domain.Providers;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Sources;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Tags;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Domain.Taxonomies;
using SchoolAccount.Domain.Types;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<TaskEntity> Tasks { get; }
    DbSet<SubTaskEntity> SubTasks { get; }
    DbSet<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; set; }
    DbSet<TagsSourceMappingEntity> TagsSourceMappings { get; set; }
    DbSet<SchoolTypeEntity> SchoolTypes { get; set; }
    DbSet<SourceEntity> Sources { get; set; }
    DbSet<TagEntity> Tags { get; set; }
    DbSet<TaxonomyEntity> Taxonomies { get; set; }
    DbSet<TaxonomyGroupingEntity> TaxonomyGroupings { get; set; }
    DbSet<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; set; }
    DbSet<TypeEntity> Types { get; set; }
    DbSet<TypeGroupingEntity> TypeGroupings { get; set; }
    DbSet<TypeTaskMappingEntity> TypeTaskMappings { get; set; }
    DbSet<ProviderOverrideEntity> ProviderOverrides { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
