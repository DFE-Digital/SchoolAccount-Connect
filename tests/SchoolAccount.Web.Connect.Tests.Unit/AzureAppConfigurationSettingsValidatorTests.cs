using FluentValidation.TestHelper;
using SchoolAccount.Web.Connect.Settings;
using SchoolAccount.Web.Connect.Validation.Validators;
using Xunit;

namespace SchoolAccount.Web.Connect.Tests.Unit;

public class AzureAppConfigurationSettingsValidatorTests
{
    private readonly AzureAppConfigurationSettingsValidator _validator = new();

    [Fact]
    public void Disabled_azure_app_configuration_is_valid()
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = false,
            IsEmulated = false,
            Endpoint = string.Empty,
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("http://app-config-emulator:8483")]
    [InlineData("https://myconfig.azconfig.io")]
    [InlineData("http://localhost:8483")]
    [InlineData("https://example.com:443/path")]
    public void Enabled_configuration_with_valid_direct_uri_is_valid(string endpoint)
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = endpoint,
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(
        "Endpoint=http://app-config-emulator:8483;Id=emulator-test-id;Secret=abcdefghijklmnopqrstuvwxyz1234567890"
    )]
    [InlineData("Endpoint=https://myconfig.azconfig.io;Id=test;Secret=secret")]
    [InlineData("Endpoint=http://localhost:8483;Id=id;Secret=secret")]
    public void Enabled_configuration_with_valid_connection_string_is_valid(string connectionString)
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = connectionString,
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("not-a-uri")]
    [InlineData("ftp://invalid-scheme.com")]
    [InlineData("")]
    [InlineData("   ")]
    public void Enabled_configuration_with_invalid_direct_uri_is_invalid(string endpoint)
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = endpoint,
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Endpoint).WithErrorMessage("Endpoint must be a valid URI");
    }

    [Theory]
    [InlineData("Endpoint=invalid-uri;Id=test;Secret=secret")]
    [InlineData("Endpoint=ftp://invalid-scheme.com;Id=test;Secret=secret")]
    public void Enabled_configuration_with_invalid_connection_string_is_invalid(string connectionString)
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = connectionString,
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Endpoint).WithErrorMessage("Endpoint must be a valid URI");
    }

    [Fact]
    public void Enabled_configuration_with_empty_endpoint_is_invalid()
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = string.Empty,
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result
            .ShouldHaveValidationErrorFor(x => x.Endpoint)
            .WithErrorMessage("Endpoint must not be empty when enabled");
    }

    [Fact]
    public void Enabled_configuration_with_connection_string_having_multiple_semicolons_is_valid()
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = "Endpoint=http://app-config-emulator:8483;Id=emulator-test-id;Secret=abc;Extra=value",
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Enabled_configuration_with_case_insensitive_endpoint_prefix_is_valid()
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = "endpoint=http://localhost:8483;Id=test;Secret=secret",
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("http://app-config-emulator:8483")]
    [InlineData("https://config.azure.com")]
    [InlineData("http://192.168.1.1:8483")]
    public void Enabled_configuration_with_various_valid_uri_formats_is_valid(string endpoint)
    {
        // Arrange
        var settings = new AzureAppConfigurationSettings
        {
            Enabled = true,
            IsEmulated = true,
            Endpoint = endpoint,
        };

        // Act
        var result = _validator.TestValidate(settings);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
