namespace SchoolAccount.Web.Connect.Features.Tasks.GetAll;

public class GetAllTasksRequest
{
    public int PageSize { get; init; } = 10;

    public int PageNumber { get; init; } = 1;
}
