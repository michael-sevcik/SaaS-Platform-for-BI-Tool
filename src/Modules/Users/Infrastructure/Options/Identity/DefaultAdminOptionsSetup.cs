using BIManagement.Common.Shared.Exceptions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Users.Infrastructure.Options.Identity;

/// <summary>
/// Represents the <see cref="ConnectionStringOptions"/> setup.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConnectionStringSetup"/> class.
/// </remarks>
/// <param name="configuration">The configuration.</param>
internal sealed class DefaultAdminOptionsSetup(IConfiguration configuration) : IConfigureOptions<DefaultAdminOptions>
{
    private const string DefaultAdminGroup = "Modules:Users:DefaultAdmin";

    /// <inheritdoc />
    public void Configure(DefaultAdminOptions options)
    {
        var defaultAdminSection = configuration.GetSection(DefaultAdminGroup)
            ?? throw new InvalidConfigurationException($"Section {DefaultAdminGroup} is missing in the configuration.");

        defaultAdminSection.Bind(options);
    }
}
