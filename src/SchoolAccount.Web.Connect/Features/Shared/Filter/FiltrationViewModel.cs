using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Features.Shared.Filter;

public class FiltrationViewModel(CalendarOfItemsViewModes viewModes, Uri baseUrl, IEnumerable<Filterable> items)
    : List<Filterable>(items)
{
    public bool AreAnyItemsSelected => this.Any(x => x.Values.Any(v => v.IsSelected));
    public IEnumerable<Filterable> SelectedItems => this.Where(x => x.Values.Any(v => v.IsSelected));
    public IEnumerable<Filterable> ItemsThatShouldBeVisible =>
        this.Where(x => x.Values.Any(v => v.Count >= 1 || !v.Count.HasValue));
    public bool AnyItemsThatAreVisible => ItemsThatShouldBeVisible.Any();
    public bool AnyItems => Count > 0;

    public CalendarOfItemsViewModes ViewModes => viewModes;

    public Uri BaseUrl => baseUrl;

    public Uri GetUriWithoutFilters()
    {
        return baseUrl.RemoveQueryParamsStartingWith("filter");
    }

    public IEnumerable<Filterable> TemplatedItems => this.Where(x => x.HasTemplate());

    public static FiltrationViewModel Build(
        CalendarOfItemsViewModes viewModes,
        Uri baseUrl,
        IEnumerable<Filterable> items
    )
    {
        return new FiltrationViewModel(viewModes, baseUrl, items);
    }
}
