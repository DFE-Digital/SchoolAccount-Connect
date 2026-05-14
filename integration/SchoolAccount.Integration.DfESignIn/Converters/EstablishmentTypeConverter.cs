using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using SchoolAccount.Integration.DfESignIn.Models;
using SchoolAccount.Integration.DfESignIn.Extensions;

namespace SchoolAccount.Integration.DfESignIn.Converters;

public class EstablishmentTypeConverter : JsonConverter<EstablishmentType>
{
    public override EstablishmentType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetValueByTypeToString();

        if (int.TryParse(value, out var numericValue))
        {
            return (EstablishmentType)numericValue;
        }

        throw new JsonException($"Invalid status value: {value}");
    }

    public override void Write(Utf8JsonWriter writer, EstablishmentType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(((int)value).ToString(CultureInfo.InvariantCulture));
    }
}
