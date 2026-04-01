using System.Collections.ObjectModel;
using AwesomeAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Builders;
using X.PagedList;
using Xunit;

namespace SchoolAccount.Web.Connect.Tests.Unit.Builders;

public class CalendarOfItemsViewBuilderTests
{
    [Fact]
    public void Successfully_handles_an_empty_list_of_items()
    {
        // Arrange
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var context = Substitute.For<HttpContext>();
        var request = Substitute.For<HttpRequest>();

        httpContextAccessor.HttpContext.Returns(context);
        context.Request.Returns(request);

        var organisationContext = Substitute.For<IOrganisationContext>();
        var emptyPagedList = new StaticPagedList<CalendarOfItemsRow>(new List<CalendarOfItemsRow>(), 1, 10, 0);
        var rowViewBuilder = new CalendarOfItemsRowViewBuilder();
        var paginationViewBuilder = new PaginationViewBuilder(httpContextAccessor);
        var hostEnvironment = Substitute.For<IHostEnvironment>();
        var filters = new Collection<Filterable>();
        var viewBuilder = new CalendarOfItemsViewBuilder(
            rowViewBuilder,
            paginationViewBuilder,
            organisationContext,
            hostEnvironment,
            httpContextAccessor
        );

        MockUrl(request);

        var items = new CalendarOfItemsPagedResult(new CalendarOfItemsCriteria(), emptyPagedList, filters);

        // Act
        var viewModel = viewBuilder.BuildForPage(items, CalendarOfItemsViewModes.None);

        // Assert
        viewModel.Items.Should().BeEmpty();
    }

    private static void MockUrl(HttpRequest request)
    {
        request.Scheme.Returns("https");
        request.Host.Returns(new HostString("www.example.com"));
        request.PathBase.Returns(new PathString("/api"));
        request.Path.Returns(new PathString("/v1/users"));
        request.QueryString.Returns(new QueryString("?id=123"));
    }
}
