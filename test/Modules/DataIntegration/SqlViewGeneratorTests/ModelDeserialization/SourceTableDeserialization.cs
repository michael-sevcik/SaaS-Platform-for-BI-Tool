using BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests;
using System.Text.Json;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests.ModelDeserialization;

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
        Assert.That(deserialized, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserialized.Name, Is.EqualTo("TabMzdList"));
            Assert.That(deserialized.SelectedColumns, Is.EquivalentTo(outputColumns));
        });
    }
}