using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Deployment.Application.MetabaseDeployment;

namespace BIManagement.Modules.Deployment.Infrastructure.Metabase;

/// <summary>
/// Default implementation of the <see cref="IPreconfiguredMetabaseClientFactory"/> interface.
/// </summary>
internal class PreconfiguredMetabaseClientFactory : IPreconfiguredMetabaseClientFactory, ISingleton
{
    /// <Inheritdoc/>
    public IPreconfiguredMetabaseClient Create(string metabaseRootUrl) => new PreconfiguredMetabaseClient(metabaseRootUrl);
}
