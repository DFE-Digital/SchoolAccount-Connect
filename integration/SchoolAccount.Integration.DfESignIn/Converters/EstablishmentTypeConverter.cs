using System.Text.Json;
using System.Text.Json.Serialization;
using SchoolAccount.Integration.DfESignIn.Models;

namespace SchoolAccount.Integration.DfESignIn.Converters;

public class EstablishmentTypeConverter : JsonConverter<EstablishmentType>
{
    public override EstablishmentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (int.TryParse(value, out int numericValue))
        {
            return (EstablishmentType)numericValue;
        }

        throw new JsonException($"Invalid status value: {value}");
    }

    public override void Write(Utf8JsonWriter writer, EstablishmentType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((int)value).ToString(System.Globalization.CultureInfo.InvariantCulture));
    }
}
