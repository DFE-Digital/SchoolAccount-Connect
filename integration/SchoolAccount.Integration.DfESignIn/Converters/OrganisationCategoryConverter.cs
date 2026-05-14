using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SchoolAccount.Integration.DfESignIn.Extensions;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Converters;

public class OrganisationCategoryConverter : JsonConverter<OrganisationCategory>
{
    public override OrganisationCategory Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    )
    {
        var value = reader.GetValueByTypeToString();

        if (int.TryParse(value, out var numericValue))
        {
            return (OrganisationCategory)numericValue;
        }

        throw new JsonException($"Invalid status value: {value}");
    }

    public override void Write(Utf8JsonWriter writer, OrganisationCategory value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((int)value).ToString("D3", CultureInfo.InvariantCulture));
    }
}
