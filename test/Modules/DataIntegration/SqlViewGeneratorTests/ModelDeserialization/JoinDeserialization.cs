using BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;
using System.Text.Json;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests.ModelDeserialization;

[TestFixture]
public class JoinDeserialization : BaseModelDeserializationTests
{
    // TODO:  
    [Test]
    public void TestJoinDeserialization()
    {
        var jsonText = """
            {
              "$id": "0",
              "type": "join",
              "name": "join1",
              "joinType": "inner",
              "leftSourceEntity": {
                "$id": "1",
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
                    "$id": "5"
                  },
                  {
                    "name": "OdpracHod",
                    "description": null,
                    "dataType": {
                      "isNullable": false,
                      "simpleType": "Decimal",
                      "type": "simple"
                    },
                    "$id": "4"
                  },
                  {
                    "name": "HodSaz",
                    "description": null,
                    "dataType": {
                      "isNullable": false,
                      "simpleType": "Decimal",
                      "type": "simple"
                    },
                    "$id": "3"
                  },
                  {
                    "name": "IdObdobi",
                    "description": null,
                    "dataType": {
                      "isNullable": false,
                      "simpleType": "Integer",
                      "type": "simple"
                    },
                    "$id": "2"
                  }
                ]
              },
              "rightSourceEntity": {
                "$id": "6",
                "type": "sourceTable",
                "name": "TabMzdObd",
                "schema": null,
                "selectedColumns": [
                  {
                    "name": "MzdObd_DatumOd",
                    "description": null,
                    "dataType": {
                      "isNullable": false,
                      "simpleType": "Date",
                      "type": "simple"
                    },
                    "$id": "9"
                  },
                  {
                    "name": "MzdObd_DatumDo",
                    "description": null,
                    "dataType": {
                      "isNullable": false,
                      "simpleType": "Date",
                      "type": "simple"
                    },
                    "$id": "8"
                  },
                  {
                    "name": "IdObdobi",
                    "description": null,
                    "dataType": {
                      "isNullable": false,
                      "simpleType": "Integer",
                      "type": "simple"
                    },
                    "$id": "7"
                  }
                ]
              },
              "joinCondition": {
                "relation": "equals",
                "leftColumn": {
                  "$ref": "2"
                },
                "rightColumn": {
                  "$ref": "7"
                },
                "linkedCondition": null
              },
              "selectedColumns": [
                {
                  "$ref": "2"
                },
                {
                  "$ref": "3"
                },
                {
                  "$ref": "4"
                },
                {
                  "$ref": "5"
                },
                {
                  "$ref": "7"
                },
                {
                  "$ref": "8"
                },
                {
                  "$ref": "9"
                }
              ]
            }
            """;

        SourceColumn[] sourceColumns = [
            new("MzdObd_DatumOd", new SimpleType(SimpleType.Types.Date, false)),
            new("MzdObd_DatumDo", new SimpleType(SimpleType.Types.Date, false)),
            new("IdObdobi", new SimpleType(SimpleType.Types.Integer, false)),
        ];

        ISourceEntity sourceTable1 = new SourceTable("TabMzdList", null, [
            new("ZamestnanecId", new SimpleType(SimpleType.Types.Integer, false)),
            new("OdpracHod", new SimpleType(SimpleType.Types.Decimal, false)),
            new("HodSaz", new SimpleType(SimpleType.Types.Decimal, false)),
            new("IdObdobi", new SimpleType(SimpleType.Types.Integer, false)),
        ]);

        ISourceEntity sourceTable2 = new SourceTable("TabMzdObd", null, [
            new("MzdObd_DatumOd", new SimpleType(SimpleType.Types.Date, false)),
            new("MzdObd_DatumDo", new SimpleType(SimpleType.Types.Date, false)),
            new("IdObdobi", new SimpleType(SimpleType.Types.Integer, false)),
        ]);
        Join join = new(
            Join.Type.inner,
            sourceTable1,
            sourceTable2,
            "join1",
            [
                sourceTable1.GetColumnMapping("IdObdobi"),
                sourceTable1.GetColumnMapping("HodSaz"),
                sourceTable1.GetColumnMapping("OdpracHod"),
                sourceTable1.GetColumnMapping("ZamestnanecId"),
                sourceTable2.GetColumnMapping("IdObdobi"),
                sourceTable2.GetColumnMapping("MzdObd_DatumDo"),
                sourceTable2.GetColumnMapping("MzdObd_DatumOd"),
            ],
            new(JoinCondition.Operator.equals, sourceTable2.GetColumnMapping("IdObdobi"), sourceTable2.GetColumnMapping("IdObdobi")));

        var deserialized = JsonSerializer.Deserialize<Join>(jsonText, MappingJsonOptions.CreateOptions());
        //deserialized?.AssignColumnOwnership();

        Assert.That(deserialized, Is.Not.Null);
        AreEqualByJson(deserialized, join);
    }
}
