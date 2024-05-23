using BIManagement.Common.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Users.Infrastructure;

public class UsersModuleInstaller : IModuleInstaller
{
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {

        IModuleInstaller a = new UsersModuleInstaller();
        // TODO: 
        throw new NotImplementedException();
    }
}
