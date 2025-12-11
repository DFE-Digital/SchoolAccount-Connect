using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolAccount.Web.Manage.Models;

public class RadioListItem : SelectListItem
{
    public string? Hint { get; private set; }

    public void SetHint(string? hint)
    {
        Hint = hint;
    }
}