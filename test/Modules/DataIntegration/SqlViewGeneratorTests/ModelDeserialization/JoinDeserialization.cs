using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators.Conditions;
using System.Text.Json;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests.ModelDeserialization;

[TestFixture]
public class JoinDeserialization : BaseModelDeserializationTests
{
    // TODO:  
    //    [Test]
    //    public void TestJoinDeserialization()
    //    {
    //        var jsonText = """
    //            {
    //                "$id" : "3",
    //                "type" : "join",
    //                "name" : "3",
    //                "joinType" : "inner",
    //                "leftSourceEntity" : {
    //                    "$id" : "1",
    //                    "type" : "sourceTable",
    //                    "name" : "TabMzdList",
    //                    "selectedColumns" : [
    //                        "ZamestnanecId", "OdpracHod", "IdObdobi"
    //                    ]
    //                },
    //                "rightSourceEntity" : {
    //                    "$id" : "2",
    //                    "type" : "sourceTable",
    //                    "name" : "TabMzdObd",
    //                    "selectedColumns" : [
    //                        "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi"
    //                    ]
    //                },
    //                "joinCondition" : {
    //                        "leftColumn" : {
    //                            "sourceEntity" : {
    //                                "$ref" : "1"
    //                            },
    //                            "sourceColumn" : "IdObdobi"
    //                        },
    //                        "rightColumn" : {
    //                            "sourceEntity" : {
    //                                "$ref" : "2"
    //                            },
    //                            "sourceColumn" : "IdObdobi"
    //                        },
    //                        "relation" : "="
    //                },
    //                "outputColumns" : [
    //                    {
    //                        "sourceEntity" : {
    //                            "$ref" : "1"
    //                        },
    //                        "sourceColumn" : "ZamestnanecId"
    //                    },
    //                    {
    //                        "sourceEntity" : {
    //                            "$ref" : "1"
    //                        },
    //                        "sourceColumn" : "OdpracHod"
    //                    },
    //                    {
    //                        "sourceEntity" : {
    //                            "$ref" : "2"
    //                        },
    //                        "sourceColumn" : "MzdObd_DatumOd"
    //                    },
    //                    {
    //                        "sourceEntity" : {
    //                            "$ref" : "2"
    //                        },
    //                        "sourceColumn" : "MzdObd_DatumDo"
    //                    }
    //                ]
    //            }
    //            """;

    //        var deserialized = JsonSerializer.Deserialize<Join>(jsonText, SerializerOptions);

    //        Assert.That(deserialized, Is.Not.Null);
    //    }

    //    [Test]
    //    public void TestJoinSerialization()
    //    {
    //        SourceTable left = new("TabMzdList", ["ZamestnanecId", "OdpracHod", "IdObdobi"]);
    //        SourceTable right = new("TabMzdObd", ["MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi"]);

    //        Join join = new(
    //            Join.Type.Inner,
    //            left,
    //            right,
    //            "3",
    //            new SourceColumn[] { new(left, "IdObdobi"), new(right, "IdObdobi") },
    //            new(
    //                JoinCondition.Operator.Equal,
    //                new(left, "IdObdobi"),
    //                new(right, "IdObdobi")));

    //        var serialized = JsonSerializer.Serialize(join, SerializerOptions);
    //        var serializedISource = JsonSerializer.Serialize<ISourceEntity>(join, SerializerOptions);
    //        Assert.Multiple(() =>
    //        {
    //            Assert.That(serialized, Is.Not.Null);
    //            Assert.That(serializedISource, Is.Not.Null);
    //        });
    //    }

    //    [Test]
    //    public void TestJoinConditionDeserialization()
    //    {
    //        var jsonText = """
    //            {
    //                "leftColumn" : {
    //                    "sourceEntity" : {
    //                        "$id" : "1",
    //                        "type" : "sourceTable",
    //                        "name" : "TabMzdList",
    //                        "selectedColumns" : [
    //                            "ZamestnanecId", "OdpracHod", "IdObdobi"
    //                        ]
    //                    },
    //                    "sourceColumn" : "IdObdobi"
    //                },
    //                "rightColumn" : {
    //                    "sourceEntity" : {
    //                        "$id" : "2",
    //                        "type" : "sourceTable",
    //                        "name" : "TabMzdObd",
    //                        "selectedColumns" : [
    //                            "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi"
    //                        ]
    //                    },
    //                    "sourceColumn" : "IdObdobi"
    //                },
    //                "relation" : "equal",
    //                "linkedCondition" : null
    //            }
    //            """;

    //        var deserialized = JsonSerializer.Deserialize<JoinCondition>(jsonText, SerializerOptions);

    //        Assert.That(deserialized, Is.Not.Null);
    //    }
}
