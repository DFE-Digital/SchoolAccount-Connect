using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Filters;

public class SubTaskFilterableFactory(
    IApplicationDbContext applicationDbContext
) : IFilterableFactory<CalendarOfItemsRow>
{
    private static List<FilterableItem> BuildTagTree(List<TagEntity> tags, List<Tuple<long?, IEnumerable<long>>>? byTags = null)
    {
        return tags
            .Select(x => new FilterableItem()
            {
                DisplayName = x.DisplayName!,
                Value = x.Id.ToString(Thread.CurrentThread.CurrentCulture),
                Count = byTags?.Count(t => t.Item2.Any(c => c == x.Id)) ?? null
            })
            .ToList();
    }

    private static List<FilterableItem> BuildTypeTree(List<TypeEntity> types, int? parentId = null,
        List<Tuple<long?, IEnumerable<long>>>? byTypes = null)
    {
        return types
            .Where(c => c.ParentTypeId == parentId)
            .Select(c => new FilterableItem()
            {
                DisplayName = c.DisplayName ?? c.Name,
                Value = c.Id.ToString(Thread.CurrentThread.CurrentCulture),
                Children = BuildTypeTree(types, c.Id).ToCollection(),
                Count = byTypes?.Count(t => t.Item2.Any(t => t == c.Id)) ?? null
            })
            .ToList();
    }

    public bool IsCreatorFor(FilterableEntities identifier)
    {
        return (identifier & FilterableEntities.SubTask) == FilterableEntities.SubTask;
    }

    public async Task<List<Filterable>> GetAvailableFiltersAsync(IQueryable<CalendarOfItemsRow>? baseQuery = null)
    {
        var items = new List<Filterable>();

        #region Phase of Education

        var byTags = baseQuery is not null
            ? await baseQuery
                .Select(x => Tuple.Create(
                    x.Id,
                    x.Tags.Select(t => t.Id)))
                .ToListAsync()
            : null;

        items.Add(new Filterable(SubTaskFilterableRegistrar.Keys.PhaseOfEducation, "Phase of education")
        {
            Values = BuildTagTree(
                await applicationDbContext.Tags
                    .Where(x => x.Taxonomy.Name == TaxonomyEntity.IdValues.PhaseOfEducation)
                    .ToListAsync())
                .ToCollection()
        });

        #endregion

        #region Categories

        var byTypes =
            baseQuery is not null
                ? await baseQuery
                    .Select(x => Tuple.Create(
                        x.Id,
                        x.Types.Select(t => t.Id)))
                    .ToListAsync()
                : null;

        items.Add(new Filterable(SubTaskFilterableRegistrar.Keys.Categories, "Categories")
        {
            Values = BuildTypeTree(
                await applicationDbContext.Types
                    .Where(x => x.ParentTypeId == null)
                    .ToListAsync())
                .ToCollection()
        });

        #endregion

        return items;
    }
}