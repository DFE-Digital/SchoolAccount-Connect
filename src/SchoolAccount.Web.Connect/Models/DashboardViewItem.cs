namespace SchoolAccount.Web.Connect.Models;

public record DashboardViewItem(string View)
{
    public DashboardViewItem(string view, object model)
        : this(view)
    {
        ViewModel = model;
    }

    public object? ViewModel { get; init; }
}
