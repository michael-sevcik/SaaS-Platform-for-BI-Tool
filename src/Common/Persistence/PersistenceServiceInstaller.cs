﻿using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.Common.Persistence.Options;
using BIManagement.Common.Shared.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BIManagement.Common.Persistence;

/// <summary>
/// Represents the persistence service installer.
/// </summary>
internal sealed class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public static void Install(IServiceCollection services, IConfiguration configuration) =>
        services
            //.AddMemoryCache() // TODO: CHECK IF THIS WILL BE USEFUL
            .ConfigureOptions<ConnectionStringSetup>()
            .AddTransientAsMatchingInterfaces(AssemblyReference.Assembly);
}
