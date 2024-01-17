using SqlViewGenerator.JsonModel;
using SqlViewGenerator.JsonModel.Agregators;
using SqlViewGenerator.MappingParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SqlViewGeneratorTests;

public class BaseModelDeserializationTests
{
    // TODO: Create an entity from this which parses
    protected JsonSerializerOptions SerializerOptions => new(JsonSerializerDefaults.Web)
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

    public static void AreEqualByJson(object expected, object actual)
    {
        var expectedJson = JsonSerializer.Serialize(expected);
        var actualJson = JsonSerializer.Serialize(actual);

        Assert.IsNotNull(expectedJson);
        Assert.IsNotNull(actualJson);
        Assert.That(actualJson, Is.EqualTo(expectedJson));
    }
}
