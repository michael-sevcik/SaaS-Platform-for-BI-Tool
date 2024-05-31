using BIManagement.Common.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Common.Infrastructure.Extensions;

namespace BIManagement.Modules.Deployment.Infrastructure;

public class DeploymentModuleInstaller : IModuleInstaller
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
}
