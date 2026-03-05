using System.Text.Json;
using System.Text.Json.Serialization;

namespace SchoolAccount.Integration.DfESignIn.Converters;

public class OrganisationCategoryConverter : JsonConverter<OrganisationCategory>
{
    public override OrganisationCategory Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (int.TryParse(value, out int numericValue))
        {
            return (OrganisationCategory)numericValue;
        }

        throw new JsonException($"Invalid status value: {value}");
    }

    public override void Write(Utf8JsonWriter writer, OrganisationCategory value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((int)value).ToString("D3", System.Globalization.CultureInfo.InvariantCulture));
    }
}