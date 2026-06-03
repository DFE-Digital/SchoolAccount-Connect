using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Domain.Tags;
using SchoolAccount.Domain.Taxonomies;
using SchoolAccount.Domain.Types;

namespace SchoolAccount.Application.Features.Shared.Filtering.Filters;

public class SubTaskFilterableFactory(IApplicationDbContext applicationDbContext) : IFilterableFactory
{
    public async Task<List<Filterable>> GetAvailableFiltersAsync<TRow>(IQueryable<TRow>? baseQuery = null)
        where TRow : IQueryRow
    {
        var items = new List<Filterable>();

        #region Phase of Education

        var byTags = baseQuery is not null ? await baseQuery.Select(x => x.Tags.Select(t => t.Id)).ToListAsync() : null;

        items.Add(
            new Filterable(SubTaskFilterableRegistrar.Keys.PhaseOfEducation, "Phase of education")
            {
                Values = BuildTagTree(
                    await applicationDbContext
                        .Tags.Where(x => x.Taxonomy.Name == TaxonomyEntity.IdValues.PhaseOfEducation)
                        .ToListAsync(),
                    byTags
                ),
            }
        );

        #endregion
        #region Categories

        var byTypes = baseQuery is not null
            ? await baseQuery.Select(x => x.Types.Select(t => t.Id)).ToListAsync()
            : null;

        items.Add(
            new Filterable(SubTaskFilterableRegistrar.Keys.Categories, "Categories")
            {
                Values = BuildTypeTree(
                    await applicationDbContext
                        .Types.Where(TypeSpecifications.OnlyActiveHubTypes())
                        .Where(TypeSpecifications.TopLevelOnly())
                        .ToListAsync(),
                    byTypes: byTypes
                ),
            }
        );

        #endregion

        return items;
    }

    private static Collection<FilterableItem> BuildTagTree(List<TagEntity> tags, List<IEnumerable<long>>? byTags = null)
    {
        return tags.Select(x => new FilterableItem()
            {
                DisplayName = x.DisplayName!,
                Value = x.Id.ToString(Thread.CurrentThread.CurrentCulture),
                Count = byTags?.Count(t => t.Any(c => c == x.Id)) ?? null,
            })
            .ToCollection();
    }

    private static Collection<FilterableItem> BuildTypeTree(
        List<TypeEntity> types,
        int? parentId = null,
        List<IEnumerable<long>>? byTypes = null
    )
    {
        return types
            .Where(c => c.ParentTypeId == parentId)
            .Select(c => new FilterableItem()
            {
                DisplayName = c.DisplayName ?? c.Name,
                Value = c.Id.ToString(Thread.CurrentThread.CurrentCulture),
                Children = BuildTypeTree(types, c.Id).ToCollection(),
                Count = byTypes?.Count(t => t.Any(x => x == c.Id)) ?? null,
            })
            .ToCollection();
    }
}
