using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Modules.Users.Domain;
using BIManagement.Modules.Users.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Users.Infrastructure;

public class UsersModuleInstaller : IModuleInstaller
{
    public static void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


        // TODO: FINISH installer
        throw new NotImplementedException();
    }
}
