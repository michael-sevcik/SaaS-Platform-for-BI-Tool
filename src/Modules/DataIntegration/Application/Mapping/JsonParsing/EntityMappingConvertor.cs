using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;

/// <summary>
/// Converter for the <see cref="EntityMapping"/> class.
/// </summary>
internal class EntityMappingConvertor : JsonConverter<EntityMapping>
{
    /// <summary>
    /// Deserializes the <see cref="EntityMapping"/> from the JSON.
    /// </summary>
    /// <param name="reader">The reader of JSON input.</param>
    /// <param name="typeToConvert">Type to convert to.</param>
    /// <param name="options">Options for deserialization.</param>
    /// <returns>Instance of deserialized <see cref="EntityMapping"/>.</returns>
    /// <exception cref="JsonException">Missing properties or unexpected JSON value tyoes.</exception>
    /// <exception cref="KeyNotFoundException">Missing property.</exception>
    /// <exception cref="InvalidOperationException">Invalid JSON value type.</exception>
    public override EntityMapping? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
       
        using var doc = JsonDocument.ParseValue(ref reader);
        var name = doc.RootElement.GetProperty("name").GetString()
            ?? throw new JsonException("Property \"name\" is missing or has invalid type");

        var schema = doc.RootElement.TryGetProperty("schema", out var schemaProperty)
            ? schemaProperty.GetString() : null;

        var description = doc.RootElement.TryGetProperty("description", out var descriptionProperty)
            ? descriptionProperty.GetString() : null;

        var mappingData = doc.RootElement.GetProperty("mappingData");
        if (mappingData.ValueKind != JsonValueKind.Array)
        {
            throw new JsonException("Property \"mappingData\" is missing or has invalid type");
        }

        var plainSourceEntities = mappingData[0];
        var plainRootSourceEntity = mappingData[1];
        var plainTargetColumnMappings = mappingData[2];

        var sourceEntities = JsonSerializer.Deserialize<ISourceEntity[]>(plainSourceEntities, options)
            ?? throw new JsonException("Property \"sourceEntities\" is missing or has invalid type");

        var rootSourceEntity = JsonSerializer.Deserialize<ISourceEntity>(plainRootSourceEntity, options);
        var targetColumnMappings = JsonSerializer.Deserialize<Dictionary<string, SourceColumn?>>(plainTargetColumnMappings, options)
            ?? throw new JsonException("Property \"targetColumnMappings\" is missing or has invalid type");

        return new(name, schema, sourceEntities, rootSourceEntity, targetColumnMappings, description);
    }

    /// <summary>
    /// Writes serialized <see cref="EntityMapping"/> to the <paramref name="writer"/>.
    /// </summary>
    /// <param name="writer">Writer of JSON representation.</param>
    /// <param name="value">Instance of entity mapping to serialize.</param>
    /// <param name="options">Options for serialization.</param>
    public override void Write(Utf8JsonWriter writer, EntityMapping value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("name");
        writer.WriteStringValue(value.Name);

        writer.WritePropertyName("schema");
        writer.WriteStringValue(value.Schema);

        writer.WritePropertyName("description");
        writer.WriteStringValue(value.Description);

        writer.WritePropertyName("mappingData");
        writer.WriteStartArray();
        JsonSerializer.Serialize(writer, value.SourceEntities, options);
        JsonSerializer.Serialize(writer, value.SourceEntity, options);
        JsonSerializer.Serialize(writer, value.ColumnMappings, options);
        writer.WriteEndArray();

        writer.WriteEndObject();
    }
}
