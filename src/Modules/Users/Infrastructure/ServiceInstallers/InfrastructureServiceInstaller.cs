using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.Modules.Users.Infrastructure.Options.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Users.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the user's application layer.
/// </summary>
internal class InfrastructureServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.ConfigureOptions<DefaultAdminOptionsSetup>();
}
