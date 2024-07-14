using BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests.ModelDeserialization;

[TestFixture]
public class ISourceEntitiyDeserializetion : BaseModelDeserializationTests
{
    [Test]
    public void TestISourceEntityDeserialization()
    {
        var jsonText = """
        {
            "$id": "0",
            "type": "sourceTable",
            "name": "TabMzdList",
            "schema": null,
            "selectedColumns": [
            {
                "name": "ZamestnanecId",
                "description": null,
                "dataType": {
                "isNullable": false,
                "simpleType": "Integer",
                "type": "simple"
                },
                "$id": "4"
            },
            {
                "name": "OdpracHod",
                "description": null,
                "dataType": {
                "isNullable": false,
                "simpleType": "Decimal",
                "type": "simple"
                },
                "$id": "3"
            },
            {
                "name": "HodSaz",
                "description": null,
                "dataType": {
                "isNullable": false,
                "simpleType": "Decimal",
                "type": "simple"
                },
                "$id": "2"
            },
            {
                "name": "IdObdobi",
                "description": null,
                "dataType": {
                "isNullable": false,
                "simpleType": "Integer",
                "type": "simple"
                },
                "$id": "1"
            }
            ]
        }
        """;

        SourceColumn[] sourceColumns = [
            new("ZamestnanecId", new SimpleType(SimpleType.Types.Integer, false)),
            new("OdpracHod", new SimpleType(SimpleType.Types.Decimal, false)),
            new("HodSaz", new SimpleType(SimpleType.Types.Decimal, false)),
            new("IdObdobi", new SimpleType(SimpleType.Types.Integer, false)),
        ];

        SourceTable sourceTable = new("TabMzdList", null, sourceColumns);

        var deserialized = JsonSerializer.Deserialize<ISourceEntity>(jsonText, MappingJsonOptions.CreateOptions());
        Assert.That(deserialized, Is.Not.Null);
        AreEqualByJson(sourceTable, deserialized);
    }
}
