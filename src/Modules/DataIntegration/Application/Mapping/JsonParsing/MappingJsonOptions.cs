using System.Text.Json;
using System.Text.Json.Serialization;

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;

public static class MappingJsonOptions 
{
    public static JsonSerializerOptions CreateOptions() => new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new SourceEntitiyConvertor(),
            new JsonStringEnumConverter(),
            new EntityMappingConvertor()
        },
        ReferenceHandler = new SourceEntityReferenceHandler(),
        WriteIndented = true,
    };
}
