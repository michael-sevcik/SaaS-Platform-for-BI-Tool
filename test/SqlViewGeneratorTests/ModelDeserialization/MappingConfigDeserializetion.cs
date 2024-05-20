using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel;
using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators;
using BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel.Agregators.Conditions;

namespace SqlViewGeneratorTests.ModelDeserialization;

[TestFixture]
public class MappingConfigDeserializetion : BaseModelDeserializationTests
{
    [Test]
    public void TestMappingConfigDeserialization()
    {
        DbConnectionConfig dbConnectionConfig = new("SQL Server", "server", "db");

        ISourceEntity sourceTable1 = new SourceTable("TabMzdList", new string[] { "ZamestnanecId", "OdpracHod", "IdObdobi" });
        ISourceEntity sourceTable2 = new SourceTable("TabMzdObd", new string[] { "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi" });
        Join join = new(
            Join.Type.Inner,
            sourceTable1,
            sourceTable2,
            "3",
            new ColumnMapping[] {
                sourceTable1.GetColumnMapping("ZamestnanecId"),
                sourceTable1.GetColumnMapping("OdpracHod"),
                sourceTable2.GetColumnMapping("MzdObd_DatumOd"),
                sourceTable2.GetColumnMapping("MzdObd_DatumDo"),
            },
            new(JoinCondition.Operator.Equal, sourceTable1.GetColumnMapping("IdObdobi"), sourceTable2.GetColumnMapping("IdObdobi")));

        EntityMapping entityMapping = new(
            name: "EmployeeHoursWorked",
            sourceEntities: new ISourceEntity[] { sourceTable1, sourceTable2, join },
            join,
            new() {
                { "PersonalId", sourceTable1.GetColumnMapping("ZamestnanecId")},
                { "HoursCount", sourceTable1.GetColumnMapping("OdpracHod")},
                { "DateFrom", sourceTable2.GetColumnMapping("MzdObd_DatumOd")},
                { "DateTo", sourceTable2.GetColumnMapping("MzdObd_DatumDo")},
            });

        MappingConfig expectedMappingConfig = new(dbConnectionConfig, new EntityMapping[] { entityMapping });

        var jsonText = """
                        {
                "sourceConnection" : {
                    "databaseProvider" : "SQL Server",
                    "server" : "server",
                    "initialCatalog" : "db"
                },
                "targetMappings" : [
                    {
                        "name" : "EmployeeHoursWorked",
                        "sourceEntity" : {
                            "$ref" : "3"
                        },
                        "SourceEntities" : [
                            {
                                "$id" : "1",
                                "type" : "sourceTable",
                                "name" : "TabMzdList",
                                "selectedColumns" : [
                                    "ZamestnanecId", "OdpracHod", "IdObdobi"
                                ]
                            },
                            {
                                "$id" : "2",
                                "type" : "sourceTable",
                                "name" : "TabMzdObd",
                                "selectedColumns" : [
                                    "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi"
                                ]
                            },
                            {
                                "$id" : "3",
                                "type" : "join",
                                "name" : "3",
                                "joinType" : "inner",
                                "leftSourceEntity" : {
                                    "$ref" : "1"
                                },
                                "rightSourceEntity" : {
                                    "$ref" : "2"
                                },
                                "joinCondition" : {
                                        "leftColumn" : {
                                            "sourceEntity" : {
                                                "$ref" : "1"
                                            },
                                            "sourceColumn" : "IdObdobi"
                                        },
                                        "rightColumn" : {
                                            "sourceEntity" : {
                                                "$ref" : "2"
                                            },
                                            "sourceColumn" : "IdObdobi"
                                        },
                                        "relation" : "equal",
                                        "linkedCondition" : null
                                },
                                "outputColumns" : [
                                    {
                                        "sourceEntity" : {
                                            "$ref" : "1"
                                        },
                                        "sourceColumn" : "ZamestnanecId"
                                    },
                                    {
                                        "sourceEntity" : {
                                            "$ref" : "1"
                                        },
                                        "sourceColumn" : "OdpracHod"
                                    },
                                    {
                                        "sourceEntity" : {
                                            "$ref" : "2"
                                        },
                                        "sourceColumn" : "MzdObd_DatumOd"
                                    },
                                    {
                                        "sourceEntity" : {
                                            "$ref" : "2"
                                        },
                                        "sourceColumn" : "MzdObd_DatumDo"
                                    }
                                ]
                            }
                        ],
                        "columnMappings" : {
                            "PersonalId" : {
                                "sourceEntity" : {
                                    "$ref" : "1"
                                },
                                "sourceColumn" : "ZamestnanecId"
                            },
                            "HoursCount" : {
                                "sourceEntity" : {
                                    "$ref" : "1"
                                },
                                "sourceColumn" : "OdpracHod"
                            },
                            "DateFrom" : {
                                "sourceEntity" : {
                                    "$ref" : "2"
                                },
                                "sourceColumn" : "MzdObd_DatumOd"
                            },
                            "DateTo" : {
                                "sourceEntity" : {
                                    "$ref" : "2"
                                },
                                "sourceColumn" : "MzdObd_DatumDo"
                            }
                        }
                    }
                ]
            }
            """;

        var deserialized = JsonSerializer.Deserialize<MappingConfig>(jsonText, SerializerOptions);

        Assert.That(deserialized, Is.Not.Null);
        AreEqualByJson(expectedMappingConfig, deserialized);
    }
}
