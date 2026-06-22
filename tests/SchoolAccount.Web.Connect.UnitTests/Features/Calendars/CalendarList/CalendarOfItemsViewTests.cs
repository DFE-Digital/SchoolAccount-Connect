using System.Collections.ObjectModel;
using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;
using Xunit;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Calendars.CalendarList;

public class CalendarOfItemsViewTests
{
    [Fact]
    public void Successfully_handles_an_empty_list_of_items()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedResult = new PagedResult<CalendarOfItemsRow>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 0,
        };

        var filters = new Collection<Filterable>();
        var viewBuilder = new CalendarOfItemsViewModelBuilder(organisationContext);
        var currentUri = new Uri("https://localhost:7033/calendar");

        var items = new CalendarOfItemsResponse(new CalendarOfItemsCriteria(), emptyPagedResult, filters);

        // Act
        var viewModel = viewBuilder.BuildForPage(items, CalendarOfItemsViewModes.None, currentUri);

        // Assert
        viewModel.Items.Should().BeEmpty();
    }
}
