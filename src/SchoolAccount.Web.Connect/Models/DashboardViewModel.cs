using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.Shared;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Models;

public class DashboardViewModel(Result outcome, Collection<DashboardViewItem> items)
    : Result<Collection<DashboardViewItem>>(items, outcome.IsSuccess, outcome.Error)
{
    private readonly Collection<DashboardViewItem> _items = items;

    public SliderCollection Slides { get; init; } = [];

    public bool ShowDividerLine(DashboardViewItem item)
    {
        var index = _items.IndexOf(item);
        return index > 0 && index < _items.Count;
    }
}
