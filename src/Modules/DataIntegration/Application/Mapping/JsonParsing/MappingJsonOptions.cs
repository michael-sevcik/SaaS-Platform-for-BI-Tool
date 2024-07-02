using System.Text.Json;
using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;

/// <summary>
/// Provides the JSON options for mapping model deserialization.
/// </summary>
public static class MappingJsonOptions
{
    /// <summary>
    /// Creates the JSON serializer options for mapping.
    /// </summary>
    /// <returns>The JSON serializer options.</returns>
    public static JsonSerializerOptions CreateOptions() => new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter(),
            new EntityMappingConvertor(),
        },
        ReferenceHandler = new MappingReferenceHandler(),
        AllowOutOfOrderMetadataProperties = true,
        WriteIndented = true,
    };
}
