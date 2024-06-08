using BIManagement.Common.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BIManagement.Common.Infrastructure.Extensions;
using BIManagement.Modules.DataIntegration.Application.DbModelling;

namespace BIManagement.Modules.DataIntegration.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents a service installer of the data integration's application layer.
/// </summary>
internal class ApplicationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc/>
    public static void Install(IServiceCollection services, IConfiguration configuration)
        => services.AddServicesWithLifetimeAsMatchingInterfaces(Application.AssemblyReference.Assembly)
            .AddScoped<IDbModelBuilderAccessor, DbModelBuilderAccessor>();

    // TODO: DELETE THIS METHOD
    ///// <summary>
    ///// Sets up EF Core design time services.
    ///// </summary>
    ///// <returns>The design time services</returns>
    ///// <exception cref="InvalidOperationException"></exception>
    //private static IDesignTimeServices GetEfCoreDesignTimeServices() 
    //{
    //    // Source: https://learn.microsoft.com/en-us/ef/core/cli/services
    //    var assemblyName = "Microsoft.EntityFrameworkCore.SqlServer";
    //    var providerAssembly = Assembly.Load(assemblyName);
    //    var providerServicesAttribute = providerAssembly.GetCustomAttribute<DesignTimeProviderServicesAttribute>()
    //        ?? throw new InvalidOperationException(
    //            $"Assembly \"{assemblyName}\" is missing required attribute \"{nameof(DesignTimeProviderServicesAttribute)}\"," +
    //            "which serves for identification of design time service provider.");

    //    var providerServicesType = providerAssembly.GetType(providerServicesAttribute.TypeName)
    //        ?? throw new InvalidOperationException($"Assembly \"{assemblyName}\" has to provide type " +
    //        $"that has \"{nameof(DesignTimeProviderServicesAttribute)}\".");

    //    var providerServices = (IDesignTimeServices)(Activator.CreateInstance(providerServicesType)
    //        ?? throw new InvalidOperationException($"EF Core design services provider type \"{providerServicesType.Name}\" is not" +
    //        $" initializable through \"{nameof(Activator)}\"."));

    //    return providerServices;
    //}
}
