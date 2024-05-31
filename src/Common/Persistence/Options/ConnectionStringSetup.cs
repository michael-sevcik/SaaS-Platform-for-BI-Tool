using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BIManagement.Common.Persistence.Options;

/// <summary>
/// Represents the <see cref="ConnectionStringOptions"/> setup.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConnectionStringSetup"/> class.
/// </remarks>
/// <param name="configuration">The configuration.</param>
internal sealed class ConnectionStringSetup(IConfiguration configuration) : IConfigureOptions<ConnectionStringOptions>
{
    private const string ConnectionStringName = "Database";

    /// <inheritdoc />
    public void Configure(ConnectionStringOptions options) => options.Value = configuration.GetConnectionString(ConnectionStringName)
        ?? throw new ConfigurationException($"Parameter \"{ConnectionStringName}\" is not provided.");
}
