using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
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
        .AddServicesWithLifetimeAsMatchingInterfaces(AssemblyReference.Assembly);
}
