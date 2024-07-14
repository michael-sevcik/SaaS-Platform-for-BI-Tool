using BIManagement.Common.Shared.Exceptions;
using BIManagement.Modules.Deployment.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Deployment.Infrastructure.Options;


/// <summary>
/// Represents the <see cref="SmtpSettings"/> options setup.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SmtpSettingsOptionsSetup"/> class.
/// </remarks>
/// <param name="configuration">The configuration.</param>
internal class SmtpSettingsOptionsSetup(IConfiguration configuration) : IConfigureOptions<SmtpSettings>
{
    private const string DefaultMetabaseSmtpSettingsGroup = "Modules:Deployment:MetabaseSmtpSettings";

    public void Configure(SmtpSettings options)
    {
        var defaultAdminSection = configuration.GetSection(DefaultMetabaseSmtpSettingsGroup)
            ?? throw new InvalidConfigurationException($"Section {DefaultMetabaseSmtpSettingsGroup} is missing in the configuration.");

        defaultAdminSection.Bind(options);
    }
}
