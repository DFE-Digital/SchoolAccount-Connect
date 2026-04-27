using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Models;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Helpers;

internal static class RadioListItemHelpers
{
    private static RadioListItem? ToRadioListItem(this Reference? item, bool includeHint = true)
    {
        if (item == null)
        {
            return null;
        }

        var value = new RadioListItem
        {
            Value = item.Id.ToString(Thread.CurrentThread.CurrentCulture),
            Text = item.Name,
        };

        if (includeHint)
        {
            value.SetHint(item.Description);
        }

        return value;
    }

    internal static IEnumerable<RadioListItem> ToRadioListItems<T>(this IEnumerable<T> items, bool includeHint = true)
        where T : Reference
    {
        return items.Select(x => x.ToRadioListItem(includeHint)).OfType<RadioListItem>();
    }
}
