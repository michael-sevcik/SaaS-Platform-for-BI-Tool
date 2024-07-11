using BIManagement.Modules.Deployment.Infrastructure.Metabase;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;



namespace Infrastructure;

/// <summary>
/// Encapsulates tests for <see cref="PreconfiguredMetabaseClient"/>.
/// </summary>
public class Tests
{
    IContainer metabaseContainer;

    [SetUp]
    public async Task Setup()
    {
        metabaseContainer = new ContainerBuilder()
            .WithImage("michaelsevcik/preconfigured-metabase:1.0.0")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilMessageIsLogged("INFO metabase.task :: Task scheduler started"))
            .WithPortBinding(3000, true)
            .Build();
        await metabaseContainer.StartAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await metabaseContainer.DisposeAsync();
    }

    [Test]
    public async Task MetabaseClient_Should_ConnectToMetabase()
    {
        // Arrange
        var metabaseRootUrl = $"http://localhost:{metabaseContainer.GetMappedPublicPort(3000)}";
        var client = new PreconfiguredMetabaseClient(metabaseRootUrl);

        // Act
        var result = await client.ChangeDefaultAdminEmail("info@test.com");

        // Assert
        Assert.That(result.IsSuccess, result.Error.Message);
    }
}
