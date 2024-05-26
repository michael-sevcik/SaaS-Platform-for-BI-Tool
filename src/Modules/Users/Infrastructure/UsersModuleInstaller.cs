using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Modules.Users.Domain;
using BIManagement.Modules.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Modules.Users.Pages.Account;
using BIManagement.Common.Infrastructure.Extensions;

namespace BIManagement.Modules.Users.Infrastructure;

public class UsersModuleInstaller : IModuleInstaller
{
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        //services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        services.InstallServicesFromAssemblies(configuration,
            AssemblyReference.Assembly
            //  TODO: ADD all ADDITIONAL ASSEMBLIES
            );


        // TODO: FINISH installer
        //throw new NotImplementedException();
    }

    public static void AddEndpoints(IEndpointRouteBuilder endpoints)
        => endpoints.MapAdditionalIdentityEndpoints();
}
