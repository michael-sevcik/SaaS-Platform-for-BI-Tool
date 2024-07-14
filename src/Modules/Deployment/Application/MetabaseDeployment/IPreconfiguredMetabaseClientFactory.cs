namespace BIManagement.Modules.Deployment.Application.MetabaseDeployment;

/// <summary>
/// Represents a factory for creating preconfigured Metabase clients.
/// </summary>
public interface IPreconfiguredMetabaseClientFactory
{
    /// <summary>
    /// Creates a new preconfigured Metabase client that accesses metabase
    /// on <paramref name="metabaseRootUrl"/> with default token.
    /// </summary>
    /// <param name="metabaseRootUrl">The URL of the Metabase instance.</param>
    /// <returns>A preconfigured Metabase client.</returns>
    IPreconfiguredMetabaseClient Create(string metabaseRootUrl);
}
