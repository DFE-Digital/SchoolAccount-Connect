using SchoolAccount.Web.Connect.Models.Interfaces;

namespace SchoolAccount.Web.Connect.Models.Shared;

public sealed class PaginationItemViewModel : IPaginationItem
{
    public PaginationItemViewModel(int pageNumber, Uri uri, bool isCurrent)
    {
        PageNumber = pageNumber;
        Uri = uri;
        IsCurrent = isCurrent;
    }

    public int? PageNumber { get; init; }
    public Uri? Uri { get; init; }
    public bool IsCurrent { get; init; }
}
