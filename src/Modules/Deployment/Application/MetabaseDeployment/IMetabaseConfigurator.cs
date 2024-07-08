using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Domain.Configuration;

namespace BIManagement.Modules.Deployment.Application.MetabaseDeployment;

/// <summary>
/// Represents an interface for configuring Metabase.
/// </summary>
internal interface IMetabaseConfigurator
{
    /// <summary>
    /// Configures Metabase for the specified customer.
    /// </summary>
    /// <param name="CustomerId">The ID of the customer.</param>
    /// <param name="urlPath">The URL path of the Metabase instance.</param>
    /// <param name="adminSettings">The default admin settings for Metabase.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<Result> ConfigureMetabase(string CustomerId, string urlPath, DefaultAdminSettings adminSettings);
}
