using MockQueryable.NSubstitute;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Domain.Types;
using SchoolAccount.Tests.Common.Fakes;

namespace SchoolAccount.Application.IntegrationTests;

public class GetCategoryHubHandlerTests
{
    private readonly IApplicationDbContext _context;
    private readonly GetCategoryHubHandler _sut;

    public GetCategoryHubHandlerTests()
    {
        _context = DatabaseContext.Build();
        _sut = new GetCategoryHubHandler(_context);
    }

    [Fact]
    public async Task Returns_error_response_when_category_not_found()
    {
        // Arrange
        var categories = Array.Empty<TypeEntity>().BuildMockDbSet();
        var expectedError = GetCategoryHubErrors.NotFound(999);
        var query = new GetCategoryHubQuery(999);

        await _context.Map(
            x =>
            {
                x.Types.AddRange(categories);
            },
            CancellationToken.None
        );

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError);
    }
}
