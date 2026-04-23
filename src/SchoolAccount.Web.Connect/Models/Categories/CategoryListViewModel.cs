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
    CategoryListViewModes ViewModes,
    ICollection<CategoryListRowGroupViewModel> Categories,
    PaginationViewModel Pagination
)
{
    private readonly string _callToActionMessage = "See the full list of categories";
    
    public string? Title { get; init; }
    public bool HasTitle => !string.IsNullOrEmpty(Title);
    public string? Description { get; init; }
    public bool HasDescription => !string.IsNullOrEmpty(Description);
    public string? Heading { get; init; }
    public bool HasHeading => !string.IsNullOrEmpty(Heading);
    public string? SubHeading { get; init; }
    public bool HasSubHeading => !string.IsNullOrEmpty(SubHeading);
    public string? NoResultsMessage { get; init; }
    public bool HasNoResultsMessage => !string.IsNullOrEmpty(NoResultsMessage);
    public string? Caption { get; init; }
    public bool DisplayCaption => !string.IsNullOrEmpty(Caption) && ViewModes.HasFlag(CategoryListViewModes.Standalone);
    public bool ShowPageHeading => DisplayCaption || HasHeading || HasSubHeading;
    public bool IsStandalone => ViewModes.HasFlag(CategoryListViewModes.Standalone);
    public string HeadingStyles => "-l";
    public string SubHeadingStyles => IsStandalone ? "-l" : string.Empty;
    public bool ShowNavigator => ViewModes.HasFlag(CategoryListViewModes.None) && Pagination.PageCount > 1;
    
    public string? CallToActionMessage
    {
        get => _callToActionMessage;
        init
        {
            if (!string.IsNullOrEmpty(value))
            {
                _callToActionMessage = value;
            }
        }
    }
}
