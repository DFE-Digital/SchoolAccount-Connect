using FluentValidation.TestHelper;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Web.Connect.Features.Calendars.CalendarOfItems;
using Xunit;

namespace SchoolAccount.Web.Connect.UnitTests.Features.Calendars.CalendarOfItems;

public class CalendarOfItemsRequestValidatorTests
{
    private readonly CalendarOfItemsRequestValidator _validator = new();

    [Fact]
    public void Has_validation_error_when_view_modes_has_both_forward_and_backward()
    {
        // Arrange
        var request = new CalendarOfItemsRequest
        {
            ViewModes = CalendarOfItemsViewModes.Forward | CalendarOfItemsViewModes.Backward,
        };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result
            .ShouldHaveValidationErrorFor(x => x.ViewModes)
            .WithErrorMessage("ViewModes cannot have both Forward and Backward set.");
    }

    [Theory]
    [InlineData(CalendarOfItemsViewModes.Forward)]
    [InlineData(CalendarOfItemsViewModes.Backward)]
    [InlineData(CalendarOfItemsViewModes.None)]
    public void Passes_when_view_modes_has_at_most_one_direction(CalendarOfItemsViewModes viewModes)
    {
        // Arrange
        var request = new CalendarOfItemsRequest { ViewModes = viewModes };

        // Act
        var result = _validator.TestValidate(request);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.ViewModes);
    }
}
