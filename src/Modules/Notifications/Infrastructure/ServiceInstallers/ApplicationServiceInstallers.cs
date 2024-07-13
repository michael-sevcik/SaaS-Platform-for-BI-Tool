using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Notifications.Infrastructure.ServiceInstallers;

/// <summary>
/// Installs services from the Application layer.
/// </summary>
internal sealed class ApplicationServiceInstallers : IServiceInstaller
{
    /// <inheritdoc />
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.AddServicesWithLifetimeAsMatchingInterfaces(Application.AssemblyReference.Assembly);
}
