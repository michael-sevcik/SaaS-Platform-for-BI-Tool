using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;

/// <summary>
/// Json converter for the <see cref="DataTypeBase"/> implementations.
/// </summary>
internal class DataTypeConvertor : JsonConverter<DataTypeBase>
{
    private readonly Dictionary<string, Type> dataTypeTypesByNames = new()
    {
        { SimpleType.Descriptor, typeof(SimpleType) },
        { UnknownDataType.Descriptor, typeof(UnknownDataType) },
        { NVarChar.Descriptor, typeof(NVarChar) },
        { NVarCharMax.Descriptor, typeof(NVarCharMax) },
    };

    /// <summary>
    /// Reads the JSON representation of the object and converts it to the specified type.
    /// </summary>
    /// <param name="reader">The reader used to read the JSON data.</param>
    /// <param name="typeToConvert">The type of the object to convert.</param>
    /// <param name="options">The serializer options.</param>
    /// <returns>The converted object.</returns>
    public override DataTypeBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var readerCopy = reader;
        using var doc = JsonDocument.ParseValue(ref readerCopy);
        var typeDescriptor = doc.RootElement.GetProperty("type").GetString()
            ?? throw new JsonException("\"type\" property has invalid value type.");

        if (!dataTypeTypesByNames.TryGetValue(typeDescriptor, out var type))
        {
            throw new JsonException($"\"{typeDescriptor}\" is not a recognized type name.");
        }

        return (DataTypeBase?)JsonSerializer.Deserialize(ref reader, type, options);
    }

    /// <summary>
    /// Writes the JSON representation of the object.
    /// </summary>
    /// <param name="writer">The writer used to write the JSON data.</param>
    /// <param name="value">The object to write.</param>
    /// <param name="options">The serializer options.</param>
    public override void Write(Utf8JsonWriter writer, DataTypeBase value, JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, options);
}
