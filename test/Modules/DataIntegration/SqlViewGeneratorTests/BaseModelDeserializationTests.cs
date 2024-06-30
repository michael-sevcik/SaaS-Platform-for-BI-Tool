using BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities.Agregators;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BIManagement.Test.Modules.DataIntegration.SqlViewGeneratorTests;

public class BaseModelDeserializationTests
{
    public static void AreEqualByJson(object expected, object actual)
    {
        var expectedJson = JsonSerializer.Serialize(expected);
        var actualJson = JsonSerializer.Serialize(actual);
        Assert.Multiple(() =>
        {
            Assert.That(expectedJson, Is.Not.Null);
            Assert.That(actualJson, Is.Not.Null.And.EqualTo(expectedJson));
        });
    }
}
