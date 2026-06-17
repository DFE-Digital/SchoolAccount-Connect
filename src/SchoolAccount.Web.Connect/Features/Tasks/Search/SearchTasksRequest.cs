using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;

namespace SchoolAccount.Web.Connect.Features.Tasks.Search;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllProperties)]
public class SearchTasksRequest
{
    [FromQuery]
    public required string Term
    {
        get;
        init => field = value.Trim();
    } = string.Empty;

    [FromQuery]
    public int PageNumber { get; init; } = 1;

    [FromQuery]
    public int PageSize { get; init; } = 10;
}
