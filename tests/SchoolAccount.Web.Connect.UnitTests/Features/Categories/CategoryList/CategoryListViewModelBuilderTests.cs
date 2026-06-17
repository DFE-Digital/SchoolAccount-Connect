using AwesomeAssertions;
using SchoolAccount.Web.Connect.Features.Categories.CategoryList;
using Xunit;
using static SchoolAccount.Tests.Common.Builders.Categories.GetParentCategories.GetParentCategoriesResponseBuilder;
using static SchoolAccount.Tests.Common.Builders.Categories.GetParentCategories.GetParentCategoriesResponseCategoryBuilder;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Categories.CategoryList;

public static class CategoryListViewModelBuilderTests
{
    public class Categories
    {
        [Fact]
        public void Returns_empty_collection_when_response_has_no_categories()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel.Categories.PaginatedItems.Should().BeEmpty();
        }

        [Fact]
        public void Returns_categories_mapped_to_list_item_view_models()
        {
            // Arrange
            var category1 = AResponseCategory()
                .WithId(1)
                .WithDisplayName("Statutory Accounts")
                .WithDescription("Statutory accounts description")
                .Build();

            var category2 = AResponseCategory()
                .WithId(2)
                .WithDisplayName("Corporation Tax")
                .WithDescription("Corporation tax description")
                .Build();

            var response = AResponse().WithCategories(category1, category2);

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel
                .Categories.PaginatedItems.Skip(1)
                .Should()
                .BeEquivalentTo([
                    new
                    {
                        Name = "Statutory Accounts",
                        Url = "/categories/1",
                        Description = "Statutory accounts description",
                        OpenInNewTab = false,
                    },
                    new
                    {
                        Name = "Corporation Tax",
                        Url = "/categories/2",
                        Description = "Corporation tax description",
                        OpenInNewTab = false,
                    },
                ]);
        }

        [Fact]
        public void Returns_single_category_when_response_has_one_category()
        {
            // Arrange
            var response = AResponse()
                .WithCategories(AResponseCategory().WithId(1).WithDisplayName("Statutory Accounts"));

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel
                .Categories.PaginatedItems.Skip(1)
                .Should()
                .ContainSingle()
                .Which.Name.Should()
                .Be("Statutory Accounts");
        }

        [Fact]
        public void Categories_list_is_prepended_with_all_tasks_url()
        {
            // Arrange
            var response = AResponse()
                .WithCategories(AResponseCategory().WithId(1).WithDisplayName("Statutory Accounts"));

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel.Categories.PaginatedItems[0].Url.Should().Be(RouteConstants.Task.AllTasks);
        }

        [Fact]
        public void Category_id_maps_to_category_index_url()
        {
            // Arrange
            var response = AResponse().WithCategories(AResponseCategory().WithId(5).WithDisplayName("VAT"));

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel.Categories.PaginatedItems[1].Url.Should().Be("/categories/5");
        }

        [Fact]
        public void Description_is_null_when_category_has_no_description()
        {
            // Arrange
            var response = AResponse()
                .WithCategories(AResponseCategory().WithId(1).WithDisplayName("Statutory Accounts"));

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel.Categories.PaginatedItems[1].Description.Should().BeNull();
        }
    }

    public class TitleAndDescription
    {
        [Fact]
        public void Title_returns_explore_categories()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel.Title.Should().Be("Explore categories");
        }

        [Fact]
        public void Description_returns_expected_text()
        {
            // Arrange
            var response = AResponse();

            // Act
            var viewModel = CategoryListViewModelBuilder.Build(response);

            // Assert
            viewModel.Description.Should().Be("View required tasks and optional guidance by category.");
        }
    }
}
