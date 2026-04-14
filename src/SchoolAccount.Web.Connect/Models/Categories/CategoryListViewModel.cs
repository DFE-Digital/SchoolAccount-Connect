using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Category.Enums;

namespace SchoolAccount.Web.Connect.Models.Categories;

public class CategoryListRowGroupViewModel(string value, IEnumerable<CategoryListRowItemViewModel> items)
    : Collection<CategoryListRowItemViewModel>(items.ToCollection())
{
    public string? DisplayValue { get; } = value;
    public bool HasDisplayValue => !string.IsNullOrEmpty(DisplayValue);
}

public record CategoryListViewModel(
    string? Title,
    string? Description,
    CategoryListViewModes ViewModes,
    ICollection<CategoryListRowGroupViewModel> Categories,
    PaginationViewModel Pagination)
{
    public string? Heading { get; init; }
    public bool HasHeading => !string.IsNullOrEmpty(Heading);
    public string? SubHeading { get; init; }
    public bool HasSubHeading => !string.IsNullOrEmpty(SubHeading);
    public string? NoResultsMessage { get; init; }
    public bool HasNoResultsMessage => !string.IsNullOrEmpty(NoResultsMessage);
    public string? Caption { get; init; }
    public bool HasCaption => !string.IsNullOrEmpty(Caption);
    public bool ShowPageHeading => HasCaption || HasHeading || HasSubHeading;
}