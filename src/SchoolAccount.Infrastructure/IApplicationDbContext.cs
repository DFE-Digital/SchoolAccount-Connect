using Microsoft.EntityFrameworkCore;
using SchoolAccount.Infrastructure.Models.Entities;

namespace SchoolAccount.Infrastructure;

public interface IApplicationDbContext
{
    DbSet<TaskEntity> Tasks { get; }
    DbSet<SubTaskEntity> SubTasks { get; }
    DbSet<SchoolTypeTagMappingEntity> SchoolTypeTagMappings { get; set; }
    DbSet<TagsSourceMappingEntity> TagsSourceMappings { get; set; }
    DbSet<WorkflowStateEntity> WorkflowStates { get; set; }
    DbSet<SchoolTypeEntity> SchoolTypes { get; set; }
    DbSet<SourceEntity> Sources { get; set; }
    DbSet<TagEntity> Tags { get; set; }
    DbSet<TaxonomyEntity> Taxonomies { get; set; }
    DbSet<TaxonomyGroupingEntity> TaxonomyGroupings { get; set; }
    DbSet<TaxonomySourceAssociationEntity> TaxonomySourceAssociations { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
