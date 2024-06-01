using BIManagement.Modules.Notifications.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Notifications.Infrastructure.Email;

/// <summary>
/// Represents the <see cref="EmailOptions"/> setup.
/// </summary>
internal sealed class EmailOptionsSetup : IConfigureOptions<EmailOptions>
{
    private const string ConfigurationSectionName = "Modules:Notifications:Mail";
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailOptionsSetup"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    public EmailOptionsSetup(IConfiguration configuration) => _configuration = configuration;

    /// <inheritdoc />
    public void Configure(EmailOptions options) => _configuration.GetSection(ConfigurationSectionName).Bind(options);
}
