using BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;
using System.Text.Json;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests.ModelDeserialization;

[TestFixture]
public class EntityMappingDeserialization : BaseModelDeserializationTests
{
    [Test]
    public void TestEntityMappingDeserialization()
    {
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

        EntityMapping expectedEntityMapping = new(
            name: "EmployeeHoursWorked2",
            null,
            [sourceTable1, sourceTable2, join],
            join,
            new() {
                { "PersonalId", sourceTable1.GetColumnMapping("ZamestnanecId")},
                { "HoursCount", sourceTable1.GetColumnMapping("OdpracHod")},
                { "DateFrom", sourceTable2.GetColumnMapping("MzdObd_DatumOd")},
                { "DateTo", null},
                { "note", null}
            });

        var jsonString = """
            {
              "name": "EmployeeHoursWorked2",
              "schema": null,
              "mappingData": [
                [
                  {
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
                  {
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
                  {
                    "$id": "0",
                    "type": "join",
                    "name": "join1",
                    "joinType": "inner",
                    "leftSourceEntity": {
                      "$ref": "1"
                    },
                    "rightSourceEntity": {
                      "$ref": "6"
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
                ],
                {
                  "$ref": "0"
                },
                {
                  "PersonalId": {
                    "$ref": "5"
                  },
                  "HoursCount": {
                    "$ref": "4"
                  },
                  "DateFrom": {
                    "$ref": "9"
                  },
                  "DateTo": null,
                  "note": null
                }
              ]
            }
            """;

        var deserialized = JsonSerializer.Deserialize<EntityMapping>(jsonString, MappingJsonOptions.CreateOptions());
        //deserialized?.SourceEntity?.AssignColumnOwnership();

        Assert.That(deserialized, Is.Not.Null);
        AreEqualByJson(expectedEntityMapping, deserialized);
        //Assert.Fail();
    }


    [Test]
    public void TestEntityMappingDeserializationWithCustomQuery()
    {
        var jsonString = """
            {
              "name": "WorkReports",
              "schema": "dbo",
              "mappingData": [
                [
                  {
                    "$id": "1",
                    "type": "sourceTable",
                    "name": "TabPrikazMzdyAZmetky",
                    "schema": "dbo",
                    "selectedColumns": [
                      {
                        "name": "DatPorizeni",
                        "description": null,
                        "dataType": {
                          "isNullable": false,
                          "simpleType": "Datetime",
                          "type": "simple"
                        },
                        "$id": "11"
                      },
                      {
                        "name": "Nor_cas",
                        "description": null,
                        "dataType": {
                          "isNullable": false,
                          "simpleType": "Numeric",
                          "type": "simple"
                        },
                        "$id": "10"
                      },
                      {
                        "name": "datum",
                        "description": null,
                        "dataType": {
                          "isNullable": false,
                          "simpleType": "Datetime",
                          "type": "simple"
                        },
                        "$id": "9"
                      },
                      {
                        "name": "Zamestnanec",
                        "description": null,
                        "dataType": {
                          "isNullable": true,
                          "simpleType": "Integer",
                          "type": "simple"
                        },
                        "$id": "8"
                      },
                      {
                        "name": "kusy_zmet_opr",
                        "description": null,
                        "dataType": {
                          "isNullable": false,
                          "simpleType": "Numeric",
                          "type": "simple"
                        },
                        "$id": "7"
                      },
                      {
                        "name": "kusy_odv",
                        "description": null,
                        "dataType": {
                          "isNullable": false,
                          "simpleType": "Numeric",
                          "type": "simple"
                        },
                        "$id": "6"
                      },
                      {
                        "name": "IDPracoviste",
                        "description": null,
                        "dataType": {
                          "isNullable": true,
                          "simpleType": "Integer",
                          "type": "simple"
                        },
                        "$id": "5"
                      },
                      {
                        "name": "DokladPrPostup",
                        "description": null,
                        "dataType": {
                          "isNullable": true,
                          "simpleType": "Integer",
                          "type": "simple"
                        },
                        "$id": "4"
                      },
                      {
                        "name": "IDPrikaz",
                        "description": null,
                        "dataType": {
                          "isNullable": true,
                          "simpleType": "Integer",
                          "type": "simple"
                        },
                        "$id": "3"
                      },
                      {
                        "name": "ID",
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
                  {
                    "$id": "12",
                    "type": "customQuery",
                    "name": "WorkReportTypeCustomQuery",
                    "query": "SELECT [TabPrikazMzdyAZmetky].ID as TabPrikazMzdyAZmetkyID,\n\tCAST(\n\t\tCASE\n\t\t-- Forbidden prefixes for [TabKmenZbozi].RegCis \"17\", \"18\", \"37\", \"38\" -- and [TabKmenZbozi_EXT]._pracoviste_filtr equals one of: \"R\", \"R+cell\", \"R+podia\", \"T+R\", \"R+Přípraváři\"\n\t\tWHEN ([TabKmenZbozi].RegCis LIKE '17%' OR [TabKmenZbozi].RegCis LIKE '18%' OR [TabKmenZbozi].RegCis LIKE '37%' OR [TabKmenZbozi].RegCis LIKE '38%') AND [TabKmenZbozi_EXT]._pracoviste_filtr IN ('R', 'R+cell', 'R+podia', 'T+R', 'R+Přípraváři')\n\t\tTHEN 'Type1'\n\t\t-- Forbidden prefixes for [TabKmenZbozi].RegCis \"17\", \"18\", \"37\", \"38\" -- and [TabKmenZbozi_EXT]._pracoviste_filtr equals one of: \"TR\", \"TR+podia\", \"T+R\", \"TR+Přípraváři\"\n\t\tWHEN ([TabKmenZbozi].RegCis LIKE '17%' OR [TabKmenZbozi].RegCis LIKE '18%' OR [TabKmenZbozi].RegCis LIKE '37%' OR [TabKmenZbozi].RegCis LIKE '38%') AND [TabKmenZbozi_EXT]._pracoviste_filtr IN ('TR', 'TR+podia', 'T+R', 'TR+Přípraváři')\n\t\tTHEN 'Type2' ELSE 'Other' END AS nvarchar(100) \n\t) AS ProductType\n\tFROM [CostumerExampleData2].[dbo].[TabPrikazMzdyAZmetky]\n\tLEFT JOIN [TabKmenZbozi]\n\tON TabPrikazMzdyAZmetky.IDTabKmen = [TabKmenZbozi].ID LEFT JOIN [TabKmenZbozi_EXT] ON TabPrikazMzdyAZmetky.IDTabKmen = [TabKmenZbozi_EXT].ID",
                    "selectedColumns": [
                      {
                        "name": "ProductType",
                        "description": null,
                        "dataType": {
                          "isNullable": false,
                          "length": 100,
                          "type": "nVarChar"
                        },
                        "$id": "14"
                      },
                      {
                        "name": "TabPrikazMzdyAZmetkyID",
                        "description": null,
                        "dataType": {
                          "isNullable": false,
                          "simpleType": "Integer",
                          "type": "simple"
                        },
                        "$id": "13"
                      }
                    ]
                  },
                  {
                    "$id": "0",
                    "type": "join",
                    "name": "join_TabPrikazMzdyAZmetky_WorkReportTypeCustomQuery",
                    "joinType": "inner",
                    "leftSourceEntity": {
                      "$ref": "1"
                    },
                    "rightSourceEntity": {
                      "$ref": "12"
                    },
                    "joinCondition": {
                      "relation": "equals",
                      "leftColumn": {
                        "$ref": "2"
                      },
                      "rightColumn": {
                        "$ref": "13"
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
                        "$ref": "6"
                      },
                      {
                        "$ref": "7"
                      },
                      {
                        "$ref": "8"
                      },
                      {
                        "$ref": "9"
                      },
                      {
                        "$ref": "10"
                      },
                      {
                        "$ref": "11"
                      },
                      {
                        "$ref": "13"
                      },
                      {
                        "$ref": "14"
                      }
                    ]
                  }
                ],
                {
                  "$ref": "0"
                },
                {
                  "ID": {
                    "$ref": "2"
                  },
                  "OrderId": {
                    "$ref": "3"
                  },
                  "ProductionOperationId": {
                    "$ref": "4"
                  },
                  "Quantity": {
                    "$ref": "6"
                  },
                  "ExpectedTime": {
                    "$ref": "10"
                  },
                  "DateTime": {
                    "$ref": "11"
                  },
                  "ProductType": {
                    "$ref": "14"
                  },
                  "WorkerId": {
                    "$ref": "8"
                  },
                  "WorkPlaceId": {
                    "$ref": "5"
                  }
                }
              ]
            }
            """;

        var deserialized = JsonSerializer.Deserialize<EntityMapping>(jsonString, MappingJsonOptions.CreateOptions());

        Assert.That(deserialized, Is.Not.Null);
    }
}
