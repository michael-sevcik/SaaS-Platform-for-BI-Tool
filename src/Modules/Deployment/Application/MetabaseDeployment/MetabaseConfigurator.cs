using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Domain.Configuration;
using Microsoft.Extensions.Options;

namespace BIManagement.Modules.Deployment.Application.MetabaseDeployment;

/// <summary>
/// Default implementation of the <see cref="IMetabaseConfigurator"/> interface.
/// </summary>
/// <param name="clientFactory">The factory for creating metabase clients.</param>
internal class MetabaseConfigurator(
    IPreconfiguredMetabaseClientFactory clientFactory,
    ICustomerDatabaseSettingsAccessor databaseSettingsAccessor,
    IOptions<SmtpSettings> smtpOptions) : IMetabaseConfigurator, ISingleton
{
    /// <inheritdoc/>
    public async Task<Result> ConfigureMetabase(string CustomerId, string metabaseRootUrl, DefaultAdminSettings adminSettings)
    {
        var client = clientFactory.Create(metabaseRootUrl);

        // Cascade of configurations that will be fully executed only if all of them succeed.
        var result = await client.ChangeDefaultAdminEmail(adminSettings.Email)
            .Bind(() => client.ChangeDefaultAdminPassword(adminSettings.Password))
            .Bind(() => databaseSettingsAccessor.GetDatabaseSettings(CustomerId))
                .Map(client.ConfigureDatabaseAsync)
            .Bind(() => client.ConfigureSmtpAsync(smtpOptions.Value))
            .Bind(client.DeleteDefaultTokenAsync);

        return result;
    }
}
