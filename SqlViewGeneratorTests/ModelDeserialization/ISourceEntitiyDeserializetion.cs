using SqlViewGenerator.JsonModel;
using SqlViewGenerator.JsonModel.Agregators;
using SqlViewGenerator.MappingParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SqlViewGeneratorTests.ModelDeserialization
{
    [TestFixture]
    public class ISourceEntitiyDeserializetion : BaseModelDeserializationTests
    {
        [Test]
        public void TestISourceEntityDeserialization()
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

            // Action
            var deserialized = JsonSerializer.Deserialize<ISourceEntity>(jsonText, this.SerializerOptions);

            // Assertion
            Assert.IsNotNull(deserialized);
            Assert.That(deserialized.Name, Is.EqualTo("TabMzdList"));
            Assert.That(deserialized.SelectedColumns, Is.EquivalentTo(outputColumns));
        }

        public class X
        {
            public string Name { get; set; }

            public X? someX { get; set; }
            public X? NextX { get; set; }
        }

        [Test]
        public void TestReferenceHandlingOnSerialization()
        {
            var x = new X() { Name = "str",  };
            var x1 = new X() { Name = "str", NextX = x, someX = x };
            var serialized = JsonSerializer.Serialize(x1, this.SerializerOptions);
            Assert.IsNotNull(serialized);
        }

        [Test]
        public void TestReferenceHandlingOnDeserialization()
        {
            var jsonText = """
                {
                  "$id": "1",
                  "type": "b",
                  "name": "str",
                  "someX": {
                    "$id": "2",
                    "name": "str",
                    "someX": null,
                    "nextX": null
                  },
                  "nextX": {
                    "$ref": "2"
                  }
                }
                """;
            
            var deserialized = JsonSerializer.Deserialize<X>(jsonText, this.SerializerOptions);
            Assert.IsNotNull(deserialized);
        }
    }
}
