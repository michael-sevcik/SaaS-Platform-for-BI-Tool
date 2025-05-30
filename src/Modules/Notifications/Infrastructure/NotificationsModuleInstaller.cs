﻿using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Notifications.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Modules.Notifications.Infrastructure;

/// <summary>
/// Service installer for the notifications module.
/// </summary>
internal class NotificationsModuleInstaller : IModuleInstaller
{
    /// <inheritdoc />
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly);
}
