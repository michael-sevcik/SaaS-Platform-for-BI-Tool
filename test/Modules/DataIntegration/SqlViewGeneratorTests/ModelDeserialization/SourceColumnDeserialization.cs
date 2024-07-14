using BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests.ModelDeserialization;

public class SourceColumnDeserialization : BaseModelDeserializationTests
{
    [Test] public void Should_Deserialize_SourceColumn()
    {
        var jsonText = @"
            {
              ""name"": ""ZamestnanecId"",
              ""description"": null,
              ""dataType"": {
                ""isNullable"": false,
                ""simpleType"": ""Integer"",
                ""type"": ""simple""
              },
              ""$id"": ""4""
            }
            ";

        var sourceColumn = JsonSerializer.Deserialize<SourceColumn>(
            jsonText,
            MappingJsonOptions.CreateOptions());

        Assert.That(sourceColumn, Is.Not.Null);
        AreEqualByJson(
            new SourceColumn(
            "ZamestnanecId",
            new SimpleType(SimpleType.Types.Integer, false)),
            sourceColumn);
    }
}
