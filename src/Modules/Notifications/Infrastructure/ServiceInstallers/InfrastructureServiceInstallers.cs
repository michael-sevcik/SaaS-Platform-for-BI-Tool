using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Notifications.Application;
using BIManagement.Modules.Notifications.Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Notifications.Infrastructure.ServiceInstallers;

/// <summary>
/// Installs services from the infrastructure layer.
/// </summary>
internal sealed class InfrastructureServiceInstallers : IServiceInstaller
{
    /// <inheritdoc />
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.ConfigureOptions<EmailOptionsSetup>()
            .AddSingleton<IEmailSender, NoOpEmailSender>();
}
