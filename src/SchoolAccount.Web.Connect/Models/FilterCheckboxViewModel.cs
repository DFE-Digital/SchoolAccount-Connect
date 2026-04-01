using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.Web.Connect.Models;

public record FilterCheckboxViewModel(Filterable Filter, Collection<FilterableItem> Children)
{
    public static FilterCheckboxViewModel Parent(Filterable filterable)
    {
        return new FilterCheckboxViewModel(filterable, filterable.Values);
    }

    public static FilterCheckboxViewModel Child(Filterable filterable, Collection<FilterableItem> children)
    {
        return new FilterCheckboxViewModel(filterable, children);
    }

    public static FilterCheckboxViewModel Child(Filterable filterable, IEnumerable<FilterableItem> children)
    {
        return new FilterCheckboxViewModel(filterable, children.ToCollection());
    }
}
