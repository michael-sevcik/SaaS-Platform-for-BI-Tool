using BIManagement.Common.Infrastructure.Configuration;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Modules.Users.Pages.Account;
using BIManagement.Common.Infrastructure.Extensions;

namespace BIManagement.Modules.Users.Infrastructure;

/// <summary>
/// Service installer for the Users module.
/// </summary>
public sealed class UsersModuleInstaller : IModuleInstaller
{
    /// <inheritdoc />
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        services.InstallServicesFromAssemblies(configuration,
            AssemblyReference.Assembly
            );
    }

    /// <inheritdoc />
    public static void AddEndpoints(IEndpointRouteBuilder endpoints)
        => endpoints.MapAdditionalIdentityEndpoints();
}
