using BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests.ModelDeserialization;

public class DataTypeDeserialization : BaseModelDeserializationTests
{
    [Test]
    public void NVarChar_Should_BeDeserializable()
    {
        var jsonText = @"
            {
              ""isNullable"": true,
              ""length"": ""50"",
              ""type"": ""nVarChar""
            }
            ";

        var dataType = JsonSerializer.Deserialize<DataTypeBase>(jsonText, MappingJsonOptions.CreateOptions());

        Assert.That(dataType, Is.Not.Null);
        AreEqualByJson(new NVarChar(50, true), dataType);


    }

    [Test]
    public void SimpleDataType_Should_BeDeserializable()
    {
        var jsonText = @"
            {
              ""isNullable"": false,
              ""simpleType"": ""Integer"",
              ""type"": ""simple""
            }
            ";

        var dataType = JsonSerializer.Deserialize<DataTypeBase>(jsonText, MappingJsonOptions.CreateOptions());

        Assert.That(dataType, Is.Not.Null);
        AreEqualByJson(new SimpleType(SimpleType.Types.Integer, false), dataType);


    }
}
