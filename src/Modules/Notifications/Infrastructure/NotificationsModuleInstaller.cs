using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Notifications.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Notifications.Infrastructure;

public class NotificationsModuleInstaller : IModuleInstaller
{
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.AddSingleton<IEmailSender, NoOpEmailSender>();
}
