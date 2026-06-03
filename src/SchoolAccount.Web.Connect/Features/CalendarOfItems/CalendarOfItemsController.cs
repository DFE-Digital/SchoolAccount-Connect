using Microsoft.AspNetCore.Mvc;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfSubTasksByDirectionForTabView;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Web.Connect.Features.CalendarOfItems;

public partial class CalendarOfItemsController(
    IQueryHandler<
        GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery,
        GenericQueryPagedResult<CalendarOfItemsRow>
    > handler,
    IOrganisationContext organisationContext
) : Controller;
