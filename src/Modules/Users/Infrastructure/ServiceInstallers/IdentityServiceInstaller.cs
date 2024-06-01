using BIManagement.Common.Components.Layout;
using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Persistence.Options;
using BIManagement.Modules.Users.Domain;
using BIManagement.Modules.Users.Infrastructure.Identity;
using BIManagement.Modules.Users.Pages.Account;
using BIManagement.Modules.Users.Persistence;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Users.Infrastructure.ServiceInstallers
{
    internal class IdentityServiceInstaller : IServiceInstaller
    {
        public static void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCascadingAuthenticationState();
            services.AddScoped<IdentityUserAccessor>();
            services.AddScoped<IdentityRedirectManager>();
            services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            services.AddDbContext<UsersContext>((serviceProvider, options) => {
                ConnectionStringOptions connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()?.Value
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); ;
                options.UseSqlServer(connectionString);
            });

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UsersContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            // TODO: consider using only the notification email sender
            services.AddSingleton<IEmailSender<ApplicationUser>, Identity.IdentityNoOpEmailSender>();
            services.AddHostedService<SeedingStartupTask>();
        }
    }
}
