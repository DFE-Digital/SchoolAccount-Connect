using SchoolAccount.Application;

namespace SchoolAccount.Web.Manage.Models;

public class BasePaginationModel
{
    public int PageNumber { get; set; } = 1;
    public string? SearchTerm { get; init; }
    public int TotalItemCount { get; init; }
    public int PageSize { get; init; } = ApplicationConstants.StandardPageSize;
    public int TotalPages() => (int)Math.Ceiling((double)TotalItemCount / PageSize);
    public string? Endpoint { get; init; }
    
    public void SetCurrentPage()
    {
        if (TotalPages() >= PageNumber)
        {
            return;
        }

        PageNumber = Math.Max(1, PageNumber);
    }
}