using BIManagement.Modules.DataIntegration.DbSchemaScraping;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace BIManagement.Test.Modules.DataIntegration.DbModelling
{
    [TestFixture]
    public class MSSQLDbModelBuilderTests
    {
        private MockRepository mockRepository;



        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);


        }

        private MSSQLDbModelBuilder CreateMSSQLDbModelBuilder()
        {
            return new MSSQLDbModelBuilder();
        }

        [Test]
        public async Task CreateAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mSSQLDbModelBuilder = this.CreateMSSQLDbModelBuilder();
            DbConnectionConfiguration configuration = null;

            // Act
            var result = await mSSQLDbModelBuilder.CreateAsync(
                configuration);

            // Assert
            Assert.Fail();
            this.mockRepository.VerifyAll();
        }
    }
}
