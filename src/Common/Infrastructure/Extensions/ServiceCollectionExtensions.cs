using System.Reflection;
using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Infrastructure.Configuration;
using BIManagement.Common.Shared.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace BIManagement.Common.Infrastructure.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Installs the services using the <see cref="IServiceInstaller"/> implementations defined in the specified assemblies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="assemblies">The assemblies to scan for service installer implementations.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection InstallServicesFromAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies) =>
        services.Tap(() => assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IServiceInstaller).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ForEach(type => type
                .GetMethod(nameof(IServiceInstaller.Install))
                ?.Invoke(null, [services, configuration])));

    /// <summary>
    /// Installs the modules using the <see cref="IModuleInstaller"/> implementations defined in the specified assemblies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="assemblies">The assemblies to scan for module installer implementations.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection InstallModulesFromAssemblies(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies) =>
        services.Tap(() => ForEachModuleInstallerTypeFromAssemblies(assemblies, type => type
                .GetMethod(nameof(IModuleInstaller.Install))
                
                // null for static methods and [services, configuration] as parameters
                ?.Invoke(null, [services, configuration])));

    /// <summary>
    /// Adds module endpoints using the <see cref="IModuleInstaller"/> implementations defined in the specified assemblies.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="assemblies">The assemblies to scan for module installer implementations.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IEndpointRouteBuilder AddModulesEndpointsFromAssemblies(
        this IEndpointRouteBuilder builder,
        params Assembly[] assemblies) =>
        builder.Tap(() => ForEachModuleInstallerTypeFromAssemblies(assemblies, type => type
                    .GetMethod(nameof(IModuleInstaller.AddEndpoints))

                    // null for static methods and [services, configuration] as parameters
                    ?.Invoke(null, [builder])));


    /// <summary>
    /// Adds all of the implementations of <see cref="ITransient"/> inside the specified assembly as transient.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="assembly">The assembly to scan for transient services.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddTransientAsMatchingInterfaces(this IServiceCollection services, Assembly assembly) =>
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(filter => filter.AssignableTo<ITransient>(), false)
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithTransientLifetime());

    /// <summary>
    /// Adds all of the implementations of <see cref="IScoped"/> inside the specified assembly as scoped.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="assembly">The assembly to scan for scoped services.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddScopedAsMatchingInterfaces(this IServiceCollection services, Assembly assembly) =>
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(filter => filter.AssignableTo<IScoped>(), false)
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithTransientLifetime());

    /// <summary>
    /// Adds all of the implementations of <see cref="IScoped"/> inside the specified assembly as scoped.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="assembly">The assembly to scan for scoped services.</param>
    /// <returns>The same service collection so that multiple calls can be chained.</returns>
    public static IServiceCollection AddSigletonAsMatchingInterfaces(this IServiceCollection services, Assembly assembly) =>
        services.Scan(scan =>
            scan.FromAssemblies(assembly)
                .AddClasses(filter => filter.AssignableTo<ISigleton>(), false)
                .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                .AsMatchingInterface()
                .WithSingletonLifetime());

    public static IServiceCollection AddServicesWithLifetimeAsMatchingInterfaces(this IServiceCollection services, Assembly assembly)
        => services.AddTransientAsMatchingInterfaces(assembly)
            .AddScopedAsMatchingInterfaces(assembly)
            .AddSigletonAsMatchingInterfaces(assembly);

    private static void ForEachModuleInstallerTypeFromAssemblies(Assembly[] assemblies, Action<Type> action)
        => assemblies.SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IModuleInstaller).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ForEach(action);
}
