using BIManagement.Modules.DataIntegration.DbSchemaScraping;
using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;

namespace BIManagement.Test.Modules.DataIntegration.DbModelling
{
    [TestFixture]
    public class MSSQLDbModelFactoryTests
    {
        private MockRepository mockRepository;

        private Mock<ILogger<MSSQLDbModelFactory>> mockLogger;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockLogger = this.mockRepository.Create<ILogger<MSSQLDbModelFactory>>();
        }

        private MSSQLDbModelFactory CreateMSSQLDbModelBuilder()
        {
            return new MSSQLDbModelFactory(
                this.mockLogger.Object);
        }

        [Test]
        public async Task CreateAsync_StateUnderTest_ExpectedBehavior()
        {

            // Arrange
            var mSSQLDbModelBuilder = this.CreateMSSQLDbModelBuilder();
            CustomerDbConnectionConfiguration configuration = new()
            {
                //ConnectionString =  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebScraper;" +
                //                    "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server" +
                //                    " Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False",
                ConnectionString = "Data Source=localhost;Initial Catalog=WebScraper;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False",
                //ConnectionString = "Data Source=127.0.0.1,32770;Initial Catalog=CostumerExampleData2;User ID=sa;Password=password123!;Connect Timeout=30;" +
                //                    "Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False",
                CustomerId = "1",
                Provider = DatabaseProvider.SqlServer,
            };


            // Act
            var result = await mSSQLDbModelBuilder.CreateAsync(configuration);
            Assert.That(result.IsSuccess);

            var serializedDbModel = JsonSerializer.Serialize(result.Value, SerializationOptions.Default);
            

            // Assert
            //Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
