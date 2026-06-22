using System.Collections.ObjectModel;
using AwesomeAssertions;
using NSubstitute;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;
using X.PagedList;
using Xunit;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Calendars.CalendarList;

public class CalendarOfItemsViewModelBuilderTests
{
    [Fact]
    public void Successfully_handles_an_empty_list_of_items()
    {
        // Arrange
        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var filters = new Collection<Filterable>();
        var viewBuilder = new CalendarOfItemsViewModelBuilder(organisationContext);
        var currentUri = new Uri("https://localhost:7033/calendar");

        var items = new CalendarOfItemsPagedResult(new CalendarOfItemsCriteria(), emptyPagedList, filters);

        // Act
        var viewModel = viewBuilder.BuildForPage(items, CalendarOfItemsViewModes.None, currentUri);

        // Assert
        viewModel.Items.Should().BeEmpty();
    }
}
