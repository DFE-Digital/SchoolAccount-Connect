using SchoolAccount.Web.Connect.Models.Interfaces;

namespace SchoolAccount.Web.Connect.Models;

public sealed class PaginationItemViewModel : IPaginationItem
{
    public PaginationItemViewModel(int pageNumber, string url, bool isCurrent)
    {
        PageNumber = pageNumber;
        Url = url;
        IsCurrent = isCurrent;
    }

    public int? PageNumber { get; init; }
    public string? Url { get; init; }
    public bool IsCurrent { get; init; }
}
