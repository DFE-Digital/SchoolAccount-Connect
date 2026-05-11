using AwesomeAssertions;
using SchoolAccount.Web.Connect.Extensions;
using Xunit;

namespace SchoolAccount.Web.Connect.UnitTests.Extensions;

public class UriExtensionsTests
{
    [Fact]
    public void Adds_query_parameter_to_uri_with_no_query()
    {
        var uri = new Uri("https://example.com/path");

        var result = uri.SetQueryParam("foo", "bar");

        result.Should().Be("https://example.com/path?foo=bar");
    }

    [Fact]
    public void Adds_query_parameter_to_uri_with_existing_query()
    {
        var uri = new Uri("https://example.com/path?existing=value");

        var result = uri.SetQueryParam("foo", "bar");

        result.Should().Be("https://example.com/path?existing=value&foo=bar");
    }

    [Fact]
    public void Replaces_existing_query_parameter()
    {
        var uri = new Uri("https://example.com/path?foo=old");

        var result = uri.SetQueryParam("foo", "new");

        result.Should().Be("https://example.com/path?foo=new");
    }

    [Fact]
    public void Preserves_other_query_parameters_when_replacing()
    {
        var uri = new Uri("https://example.com/path?foo=old&other=keep");

        var result = uri.SetQueryParam("foo", "new");

        result.Should().Be("https://example.com/path?foo=new&other=keep");
    }

    [Fact]
    public void Encodes_special_characters_in_value()
    {
        var uri = new Uri("https://example.com/path");

        var result = uri.SetQueryParam("foo", "hello world");

        result.Should().Be("https://example.com/path?foo=hello%20world");
    }

    [Fact]
    public void Encodes_special_characters_in_key()
    {
        var uri = new Uri("https://example.com/path");

        var result = uri.SetQueryParam("foo bar", "baz");

        result.Should().Be("https://example.com/path?foo%20bar=baz");
    }

    [Fact]
    public void Preserves_path()
    {
        var uri = new Uri("https://example.com/some/deep/path");

        var result = uri.SetQueryParam("foo", "bar").ToString();

        result.Should().StartWith("https://example.com/some/deep/path");
    }

    [Fact]
    public void Sets_query_parameter_with_empty_value()
    {
        var uri = new Uri("https://example.com/path");

        var result = uri.SetQueryParam("foo", "");

        result.Should().Be("https://example.com/path?foo=");
    }

    [Fact]
    public void Adds_query_parameter_to_root_path_with_trailing_slash()
    {
        var uri = new Uri("https://127.0.0.1:7033/");

        var result = uri.SetQueryParam("foo", "bar");

        result.Should().Be("https://127.0.0.1:7033/?foo=bar");
    }

    [Fact]
    public void Replaces_existing_query_parameter_on_root_path_with_trailing_slash()
    {
        var uri = new Uri("https://127.0.0.1:7033/?foo=old");

        var result = uri.SetQueryParam("foo", "new");

        result.Should().Be("https://127.0.0.1:7033/?foo=new");
    }

    [Fact]
    public void Adds_query_parameter_to_root_path_without_trailing_slash()
    {
        var uri = new Uri("https://127.0.0.1:7033");

        var result = uri.SetQueryParam("foo", "bar");

        result.Should().Be("https://127.0.0.1:7033/?foo=bar");
    }

    [Fact]
    public void Throws_a_null_exception_if_uri_is_null()
    {
        Uri? uri = null;

        var act = () => uri!.SetQueryParam("foo", "bar");

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'uri')");
    }

    [Fact]
    public void Throws_an_argument_exception_if_key_is_null()
    {
        var uri = new Uri("https://example.com/path");

        var act = () => uri.SetQueryParam(null!, "bar");

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'key')");
    }

    [Fact]
    public void Removes_matching_key_and_value()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar&baz=qux");

        // Act
        var result = uri.RemoveQueryParam("foo", "bar");

        // Assert
        result.Should().Be("https://example.com/path?baz=qux");
    }

    [Fact]
    public void Removing_by_key_and_value_removes_matching_key_and_value_when_multiple_matches_exist()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar&foo=qux&baz=qry");

        // Act
        var result = uri.RemoveQueryParam("foo", "bar");

        // Assert
        result.Should().Be("https://example.com/path?foo=qux&baz=qry");
    }

    [Fact]
    public void Removing_by_key_and_value_preserves_remaining_values_as_separate_params()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar&foo=qux&foo=bert&baz=qry");

        // Act
        var result = uri.RemoveQueryParam("foo", "bar");

        // Assert
        result.Should().Be("https://example.com/path?foo=qux&foo=bert&baz=qry");
    }

    [Fact]
    public void Does_not_remove_when_key_matches_but_value_does_not()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("foo", "different");

        // Assert
        result.Should().Be("https://example.com/path?foo=bar");
    }

    [Fact]
    public void Does_not_remove_when_key_does_not_exist()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("missing", "bar");

        // Assert
        result.Should().Be("https://example.com/path?foo=bar");
    }

    [Fact]
    public void Key_matching_is_case_insensitive()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("FOO", "bar");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Preserves_other_query_params_when_removing()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar&baz=qux&quux=corge");

        // Act
        var result = uri.RemoveQueryParam("baz", "qux");

        // Assert
        result.Should().Be("https://example.com/path?foo=bar&quux=corge");
    }

    [Fact]
    public void Returns_uri_unchanged_when_no_query_string()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var result = uri.RemoveQueryParam("foo", "bar");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Removes_only_param_leaving_no_query_string()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("foo", "bar");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Preserves_path_when_removing()
    {
        // Arrange
        var uri = new Uri("https://example.com/some/deep/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("foo", "bar");

        // Assert
        result.ToString().Should().StartWith("https://example.com/some/deep/path");
    }

    [Fact]
    public void Works_on_root_path_with_trailing_slash()
    {
        // Arrange
        var uri = new Uri("https://127.0.0.1:7033/?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("foo", "bar");

        // Assert
        result.Should().Be("https://127.0.0.1:7033/");
    }

    [Fact]
    public void Removing_by_key_and_value_throws_a_null_exception_if_uri_is_null()
    {
        // Arrange
        Uri? uri = null;

        // Act
        var act = () => uri!.RemoveQueryParam("foo", "bar");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'uri')");
    }

    [Fact]
    public void Removing_by_key_and_value_throws_an_argument_exception_if_key_is_null()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var act = () => uri.RemoveQueryParam(null!, "bar");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'key')");
    }

    [Fact]
    public void Removing_by_key_and_value_throws_an_argument_exception_if_key_is_empty()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var act = () => uri.RemoveQueryParam(string.Empty, "bar");

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The value cannot be an empty string. (Parameter 'key')");
    }

    [Fact]
    public void Removes_param_by_key_regardless_of_value()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("foo");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Removes_param_by_key_preserving_other_params()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar&baz=qux");

        // Act
        var result = uri.RemoveQueryParam("foo");

        // Assert
        result.Should().Be("https://example.com/path?baz=qux");
    }

    [Fact]
    public void Removing_by_key_returns_uri_unchanged_when_key_does_not_exist()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("missing");

        // Assert
        result.Should().Be("https://example.com/path?foo=bar");
    }

    [Fact]
    public void Removing_by_key_returns_uri_unchanged_when_no_query_string()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var result = uri.RemoveQueryParam("foo");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Removing_by_key_is_case_insensitive()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("FOO");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Removing_by_key_works_on_root_path_with_trailing_slash()
    {
        // Arrange
        var uri = new Uri("https://127.0.0.1:7033/?foo=bar");

        // Act
        var result = uri.RemoveQueryParam("foo");

        // Assert
        result.Should().Be("https://127.0.0.1:7033/");
    }

    [Fact]
    public void Removing_by_key_throws_a_null_exception_if_uri_is_null()
    {
        // Arrange
        Uri? uri = null;

        // Act
        var act = () => uri!.RemoveQueryParam("foo");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'uri')");
    }

    [Fact]
    public void Removing_by_key_throws_an_argument_exception_if_key_is_null()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var act = () => uri.RemoveQueryParam(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'key')");
    }

    [Fact]
    public void Removing_by_key_throws_an_argument_exception_if_key_is_empty()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var act = () => uri.RemoveQueryParam(string.Empty);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("The value cannot be an empty string. (Parameter 'key')");
    }

    [Fact]
    public void Removes_params_with_matching_prefix()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo_one=bar&foo_two=baz&other=qux");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://example.com/path?other=qux");
    }

    [Fact]
    public void Removes_single_param_matching_prefix()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo=bar&other=qux");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://example.com/path?other=qux");
    }

    [Fact]
    public void Removes_all_params_when_all_match_prefix()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foo_one=bar&foo_two=baz");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Removing_by_prefix_returns_uri_unchanged_when_no_keys_match()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?other=qux");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://example.com/path?other=qux");
    }

    [Fact]
    public void Removing_by_prefix_returns_uri_unchanged_when_no_query_string()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://example.com/path");
    }

    [Fact]
    public void Removing_by_prefix_is_case_insensitive()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?FOO_one=bar&foo_two=baz&other=qux");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://example.com/path?other=qux");
    }

    [Fact]
    public void Removing_by_prefix_does_not_remove_partial_word_matches()
    {
        // Arrange
        var uri = new Uri("https://example.com/path?foobar=one&foo=two&barfoo=three");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://example.com/path?barfoo=three");
    }

    [Fact]
    public void Removing_by_prefix_works_on_root_path_with_trailing_slash()
    {
        // Arrange
        var uri = new Uri("https://127.0.0.1:7033/?foo_one=bar&foo_two=baz");

        // Act
        var result = uri.RemoveQueryParamsStartingWith("foo");

        // Assert
        result.Should().Be("https://127.0.0.1:7033/");
    }

    [Fact]
    public void Removing_by_prefix_throws_a_null_exception_if_uri_is_null()
    {
        // Arrange
        Uri? uri = null;

        // Act
        var act = () => uri!.RemoveQueryParamsStartingWith("foo");

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'uri')");
    }

    [Fact]
    public void Removing_by_prefix_throws_an_argument_exception_if_prefix_is_null()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var act = () => uri.RemoveQueryParamsStartingWith(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'prefix')");
    }

    [Fact]
    public void Removing_by_prefix_throws_an_argument_exception_if_prefix_is_empty()
    {
        // Arrange
        var uri = new Uri("https://example.com/path");

        // Act
        var act = () => uri.RemoveQueryParamsStartingWith(string.Empty);

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("The value cannot be an empty string. (Parameter 'prefix')");
    }
}
