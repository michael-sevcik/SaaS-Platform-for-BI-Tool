using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using SqlViewGenerator.JsonModel.Agregators;
using SqlViewGenerator.MappingParser;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SqlViewGeneratorTests.ModelDeserialization;

public class SourceTableDeserialization : BaseModelDeserializationTests
{
    [Test]
    public void DeserializationTest()
    {
        var outputColumns = new string[] { "ZamestnanecId", "OdpracHod", "IdObdobi" };
        var jsonText = """
            {
                "$id" : "1",
                "type" : "sourceTable",
                "name" : "TabMzdList",
                "selectedColumns" : [
                    "ZamestnanecId", "OdpracHod", "IdObdobi"
                ]
            }
            """;

        var deserialized = JsonSerializer.Deserialize<SourceTable>(jsonText, SerializerOptions);
        Assert.IsNotNull(deserialized);
        Assert.That(deserialized.Name, Is.EqualTo("TabMzdList"));
        Assert.That(deserialized.SelectedColumns, Is.EquivalentTo(outputColumns));

    }
}