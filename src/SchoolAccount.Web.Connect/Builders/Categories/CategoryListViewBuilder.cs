using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Enums;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Models.Categories;

namespace SchoolAccount.Web.Connect.Builders.Categories;

public class CategoryListViewBuilder(IOrganisationContext organisationContext)
{
    private readonly CategoryRowItemViewBuilder _rowViewBuilder = new();
    private readonly PaginationViewBuilder _paginationViewBuilder = new();

    public CategoryListViewModel BuildForPage(
        CategoryPagedResult items,
        CategoryListViewModes viewModes,
        Uri currentUri
    )
    {
        var options = new CategoryListViewOptions
        {
            ViewMode = viewModes,
            Title = "Explore Categories",
            Heading = "Explore Categories",
            SubHeading = "View required tasks and optional guidance by category.",
            NoResultsMessage = "No results found",
        };

        return Build(options, items, currentUri);
    }

    private CategoryListViewModel Build(CategoryListViewOptions options, CategoryPagedResult result, Uri currentUri)
    {
        Collection<CategoryListRowGroupViewModel> rows = [];

        if (result.Payload.Count > 0)
        {
            rows = result
                .Payload.GroupBy(x => options.GroupingFunction is not null ? options.GroupingFunction(x) : string.Empty)
                .Select(x => new CategoryListRowGroupViewModel(x.Key, x.Select(_rowViewBuilder.Build)))
                .ToCollection();
        }

        return new CategoryListViewModel(
            options.Title,
            options.Description,
            options.ViewMode,
            rows,
            _paginationViewBuilder.Build(result, currentUri)
        )
        {
            Caption = organisationContext.Organisation.Name,
            NoResultsMessage = options.NoResultsMessage,
            Heading = options.Heading,
            SubHeading = options.SubHeading,
            Title = options.Title,
            Description = options.Description,
        };
    }
}
