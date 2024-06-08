using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using System.Text.Json;

namespace Domain
{
    public class DbModelSerializationTests
    {
        private readonly DbModel dbModel = new()
        {
            Name = "TestDb",
            Tables =
                [
                    new()
                    {
                        Name = "TestTable",
                        Columns =
                        [
                            new ()
                            {
                                Name = "Id",
                                DataType = new SimpleType(SimpleType.Types.Integer) { IsNullable = false },
                            }
                        ]
                    }
                ]
        };

        private const string ExpectedSerialization = "{\"name\":\"TestDb\",\"tables\":[{\"name\":\"TestTable\",\"schema\":\"\",\"primaryKeys\":[],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}]}]}";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void DbModel_ShouldSerialize()
        {
            string json = JsonSerializer.Serialize(dbModel, SerializationOptions.Default);

            Assert.That(json, Is.EqualTo(ExpectedSerialization));
        }

        [Test]
        public void DbModel_ShouldDeserialize()
        {
            var dbModel = JsonSerializer.Deserialize<DbModel>(ExpectedSerialization, SerializationOptions.Default);

            var serializeDeserialization = JsonSerializer.Serialize(dbModel, SerializationOptions.Default);
            Assert.That(dbModel, Is.Not.Null);
            Assert.That(serializeDeserialization, Is.EqualTo(ExpectedSerialization));
        }
    }
}