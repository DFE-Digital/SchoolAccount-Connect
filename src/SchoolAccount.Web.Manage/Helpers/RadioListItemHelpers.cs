using SchoolAccount.Kernel;
using SchoolAccount.Web.Manage.Models;

namespace SchoolAccount.Web.Manage.Helpers;

internal static class RadioListItemHelpers
{
    internal static RadioListItem? ToRadioListItem(this Reference? item, bool includeHint = true)
    {
        if (item == null)
        {
            return null;
        }

        var value = new RadioListItem
        {
            Value = item.Id.ToString(Thread.CurrentThread.CurrentCulture),
            Text = item.Name
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
        return items
            .Select(x => x.ToRadioListItem(includeHint))
            .OfType<RadioListItem>();
    }
}