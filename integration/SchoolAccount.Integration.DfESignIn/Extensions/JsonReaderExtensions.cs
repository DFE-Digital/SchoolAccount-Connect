using System.Globalization;
using System.Text.Json;

namespace SchoolAccount.Integration.DfESignIn.Extensions;

public static class JsonReaderExtensions
{
    public static string? GetValueByTypeToString(this Utf8JsonReader reader)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt32().ToString(CultureInfo.InvariantCulture),
            JsonTokenType.String => reader.GetString(),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };
    }
}