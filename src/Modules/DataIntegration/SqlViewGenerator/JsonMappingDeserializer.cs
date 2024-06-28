namespace BIManagement.Modules.DataIntegration.SqlViewGenerator;

using System.Text.Json;
using System.Text.Json.Serialization;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators;
using BIManagement.Modules.DataIntegration.SqlViewGenerator.MappingParser;

public static class JsonMappingDeserializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new SourceEntitiyConvertor(new KeyValuePair<string, Type>[]
            {
                new ("sourceTable", typeof(SourceTable)),
                new ("join", typeof(Join)),
            }),
            new JsonStringEnumConverter(),
        },
        ReferenceHandler = new SourceEntityReferenceHandler(),
        WriteIndented = true,
    };

    internal static T? Deserialize<T>(string json)
        => JsonSerializer.Deserialize<T>(json, SerializerOptions);

    //public static MappingConfig Deserialize(string json)
    //    => Deserialize<MappingConfig>(json) ??
    //    throw new ArgumentException("The JSON string is not deserializable.", nameof(json));
}
