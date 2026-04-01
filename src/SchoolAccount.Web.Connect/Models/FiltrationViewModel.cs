using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Web.Connect.Extensions;

namespace SchoolAccount.Web.Connect.Models;

public class FiltrationViewModel(string baseUrl, IHostEnvironment env, IEnumerable<Filterable> items)
    : List<Filterable>(items)
{
    private readonly string? _baseUrl = baseUrl;

    public IHostEnvironment Environment { get; init; } = env;

    public bool AreAnyItemsSelected => this.Any(x => x.Values.Any(v => v.IsSelected));
    public IEnumerable<Filterable> SelectedItems => this.Where(x => x.Values.Any(v => v.IsSelected));
    public IEnumerable<Filterable> ItemsThatShouldBeVisible =>
        this.Where(x => x.Values.Any(v => v.Count >= 1 || !v.Count.HasValue));
    public bool AnyItemsThatAreVisible => ItemsThatShouldBeVisible.Any();
    public bool AnyItems => Count > 0;

    public string GetBaseUrl()
    {
        if (string.IsNullOrEmpty(_baseUrl))
        {
            throw new InvalidDataException(nameof(GetBaseUrl) + " is empty");
        }

        return _baseUrl;
    }

    public string GetUriWithoutFilters()
    {
        return UriExtensions.RemoveByKeyQuery(GetBaseUrl(), Environment, "filter");
    }

    public static FiltrationViewModel Build(string baseUrl, IHostEnvironment env, IEnumerable<Filterable> items)
    {
        return new FiltrationViewModel(baseUrl, env, items);
    }
}
