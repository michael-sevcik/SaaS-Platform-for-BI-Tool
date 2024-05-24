using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Modules.Users.Domain;
using BIManagement.Modules.Users.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Users.Infrastructure.ServiceInstallers
{
    internal class IdentityServiceInstaller : IServiceInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
        }
    }
}
