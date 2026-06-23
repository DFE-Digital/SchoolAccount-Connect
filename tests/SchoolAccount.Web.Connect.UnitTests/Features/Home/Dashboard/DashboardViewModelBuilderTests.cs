using AwesomeAssertions;
using SchoolAccount.Web.Connect.Features.Dashboard;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Dashboard.GetDashboard.GetDashboardResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.Dashboard.GetDashboard.GetDashboardResponseCalendarItemBuilder;
using static SchoolAccount.Tests.Common.Builders.Dashboard.GetDashboard.GetDashboardResponseCategoryItemBuilder;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Home.Dashboard;

public static class DashboardViewModelBuilderTests
{
    public class Categories
    {
        [Fact]
        public void Returns_empty_collection_when_response_has_no_categories()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.Categories.Should().BeEmpty();
        }

        [Fact]
        public void Maps_categories_to_list_item_view_models()
        {
            // Arrange
            var category1 = AResponseCategoryItem()
                .WithId(1)
                .WithDisplayName("Financial Management")
                .WithDescription("Manage your finances");

            var category2 = AResponseCategoryItem()
                .WithId(2)
                .WithDisplayName("Statutory Returns")
                .WithDescription("Complete statutory returns");

            var response = AResponse().WithCategories(category1, category2);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.Categories.Should().HaveCount(2);
            viewModel
                .Categories.Should()
                .BeEquivalentTo([
                    new
                    {
                        Name = "Financial Management",
                        Url = "/categories/1",
                        Description = "Manage your finances",
                    },
                    new
                    {
                        Name = "Statutory Returns",
                        Url = "/categories/2",
                        Description = "Complete statutory returns",
                    },
                ]);
        }

        [Fact]
        public void Maps_category_without_description()
        {
            // Arrange
            var category = AResponseCategoryItem().WithId(5).WithDisplayName("Category Without Description");

            var response = AResponse().WithCategories(category);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.Categories.Should().ContainSingle().Which.Description.Should().BeNull();
        }
    }

    public class NoCategoriesFound
    {
        [Fact]
        public void Returns_true_when_no_categories()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.NoCategoriesFound.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_categories_exist()
        {
            // Arrange
            var category = AResponseCategoryItem().WithId(1).WithDisplayName("Category");
            var response = AResponse().WithCategories(category);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.NoCategoriesFound.Should().BeFalse();
        }
    }

    public class DisplayCategoryCallToAction
    {
        [Theory]
        [InlineData(11, true)]
        [InlineData(10, false)]
        [InlineData(9, false)]
        [InlineData(1, false)]
        [InlineData(0, false)]
        public void Display_category_call_to_action_when_more_than_ten_categories(
            int numberOfCategories,
            bool expectedResult
        )
        {
            // Arrange
            var categories = BuildMany(
                Enumerable
                    .Range(1, numberOfCategories)
                    .Select(i => AResponseCategoryItem().WithId(i).WithDisplayName($"Category {i}"))
            );

            var response = AResponse().WithCategories(categories);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.DisplayCategoryCallToAction.Should().Be(expectedResult);
        }
    }

    public class CalendarGroups
    {
        [Fact]
        public void Returns_empty_collection_when_response_has_no_calendar_items()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.CalendarGroups.Should().BeEmpty();
        }

        [Fact]
        public void Groups_calendar_items_by_month()
        {
            // Arrange
            var januaryDate = new DateOnly(2024, 1, 15);
            var februaryDate = new DateOnly(2024, 2, 20);

            var item1 = AResponseCalendarItem().WithId(1).WithName("January Task").WithSortDate(januaryDate);

            var item2 = AResponseCalendarItem().WithId(2).WithName("Another January Task").WithSortDate(januaryDate);

            var item3 = AResponseCalendarItem().WithId(3).WithName("February Task").WithSortDate(februaryDate);

            var response = AResponse().WithCalendarOfItems(item1, item2, item3);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.CalendarGroups.Should().HaveCount(2);
            viewModel
                .CalendarGroups.Should()
                .ContainSingle(g => g.MonthLabel == "January 2024")
                .Which.Items.Should()
                .HaveCount(2);
            viewModel
                .CalendarGroups.Should()
                .ContainSingle(g => g.MonthLabel == "February 2024")
                .Which.Items.Should()
                .HaveCount(1);
        }

        [Fact]
        public void Maps_calendar_items_correctly()
        {
            // Arrange
            var item = AResponseCalendarItem()
                .WithId(42)
                .WithName("Submit Accounts")
                .WithDescription("Annual accounts deadline")
                .WithDateText("By 31 December 2024")
                .WithSortDate(new DateOnly(2024, 12, 31));

            var response = AResponse().WithCalendarOfItems(item);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            var calendarItem = viewModel
                .CalendarGroups.Should()
                .ContainSingle()
                .Which.Items.Should()
                .ContainSingle()
                .Subject;

            calendarItem.Name.Should().Be("Submit Accounts");
            calendarItem.Description.Should().Be("Annual accounts deadline");
            calendarItem.DateText.Should().Be("By 31 December 2024");
            calendarItem.Url.Should().Be("/tasks/42");
        }

        [Fact]
        public void Handles_items_without_sort_date()
        {
            // Arrange
            var item = AResponseCalendarItem().WithId(1).WithName("Task without date");

            var response = AResponse().WithCalendarOfItems(item);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.CalendarGroups.Should().ContainSingle().Which.MonthLabel.Should().BeEmpty();
        }
    }

    public class HasCalendarItems
    {
        [Fact]
        public void Returns_true_when_calendar_groups_exist()
        {
            // Arrange
            var item = AResponseCalendarItem().WithId(1).WithName("Task").WithSortDate(new DateOnly(2024, 1, 1));

            var response = AResponse().WithCalendarOfItems(item);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.HasCalendarItems.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_no_calendar_groups()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.HasCalendarItems.Should().BeFalse();
        }
    }

    public class CalendarLastUpdatedMessage
    {
        [Fact]
        public void Returns_formatted_message_when_items_have_last_updated_date()
        {
            // Arrange
            var lastUpdated = new DateTime(2024, 6, 15, 14, 30, 0);

            var item1 = AResponseCalendarItem()
                .WithId(1)
                .WithName("Task 1")
                .WithSortDate(new DateOnly(2024, 1, 1))
                .WithLastUpdated(new DateTime(2024, 5, 1));

            var item2 = AResponseCalendarItem()
                .WithId(2)
                .WithName("Task 2")
                .WithSortDate(new DateOnly(2024, 1, 1))
                .WithLastUpdated(lastUpdated);

            var response = AResponse().WithCalendarOfItems(item1, item2);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.CalendarLastUpdatedMessage.Should().Contain("Last updated:");
            viewModel.CalendarLastUpdatedMessage.Should().Contain("15 June 2024");
        }

        [Fact]
        public void Returns_null_when_no_items_have_last_updated_date()
        {
            // Arrange
            var item = AResponseCalendarItem().WithId(1).WithName("Task").WithSortDate(new DateOnly(2024, 1, 1));

            var response = AResponse().WithCalendarOfItems(item);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.CalendarLastUpdatedMessage.Should().BeNull();
        }

        [Fact]
        public void Returns_null_when_no_calendar_items()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.CalendarLastUpdatedMessage.Should().BeNull();
        }

        [Fact]
        public void Uses_most_recent_last_updated_date()
        {
            // Arrange
            var oldDate = new DateTime(2024, 1, 1);
            var recentDate = new DateTime(2024, 12, 31);

            var item1 = AResponseCalendarItem()
                .WithId(1)
                .WithName("Old Task")
                .WithSortDate(new DateOnly(2024, 1, 1))
                .WithLastUpdated(oldDate);

            var item2 = AResponseCalendarItem()
                .WithId(2)
                .WithName("Recent Task")
                .WithSortDate(new DateOnly(2024, 1, 1))
                .WithLastUpdated(recentDate);

            var response = AResponse().WithCalendarOfItems(item1, item2);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.CalendarLastUpdatedMessage.Should().Contain("31 December 2024");
            viewModel.CalendarLastUpdatedMessage.Should().NotContain("1 January 2024");
        }
    }

    public class HasCalendarLastUpdatedMessage
    {
        [Fact]
        public void Returns_true_when_message_exists()
        {
            // Arrange
            var item = AResponseCalendarItem()
                .WithId(1)
                .WithName("Task")
                .WithSortDate(new DateOnly(2024, 1, 1))
                .WithLastUpdated(new DateTime(2024, 6, 15));

            var response = AResponse().WithCalendarOfItems(item);

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.HasCalendarLastUpdatedMessage.Should().BeTrue();
        }

        [Fact]
        public void Returns_false_when_message_is_null()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = DashboardViewModelBuilder.Build(response);

            // Assert
            viewModel.HasCalendarLastUpdatedMessage.Should().BeFalse();
        }
    }
}
