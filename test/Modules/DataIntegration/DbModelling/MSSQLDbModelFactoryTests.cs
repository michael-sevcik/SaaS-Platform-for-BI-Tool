using BIManagement.Modules.DataIntegration.DbSchemaScraping;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using Microsoft.Extensions.Logging;
using Moq;

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
            CostumerDbConnectionConfiguration configuration = new()
            { 
                ConnectionString =  "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SaaSPlatform;" +
                                    "Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server" +
                                    " Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False",
                CostumerId = "1",
                Provider = DatabaseProvider.SqlServer,
            };


            // Act
            var result = await mSSQLDbModelBuilder.CreateAsync(
                configuration);

            // Assert
            //Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
