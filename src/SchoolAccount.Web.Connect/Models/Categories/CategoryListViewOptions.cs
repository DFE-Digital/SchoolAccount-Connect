using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.Categories.Enums;
using SchoolAccount.Application.Features.Categories.Models;

namespace SchoolAccount.Web.Connect.Models.Categories;

public class CategoryListViewOptions
{
    public CategoryListViewModes ViewMode { get; set; }
    public string? Title { get; init; } = null!;
    public Collection<CategoryType>? Categories { get; init; }
    public Func<CategoryRow, string>? GroupingFunction { get; init; }
    public string? Description { get; init; } = null!;
    public string? BaseUri { get; set; }
    public string? NoResultsMessage { get; init; }
    public string? Heading { get; init; }
    public string? SubHeading { get; init; }
    public string? CallToActionMessage { get; init; }
}
