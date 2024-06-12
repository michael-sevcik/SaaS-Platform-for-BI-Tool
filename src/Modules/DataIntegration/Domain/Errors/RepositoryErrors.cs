using BIManagement.Common.Shared.Errors;

namespace BIManagement.Modules.DataIntegration.Domain.Errors;

/// <summary>
/// Encapsulates errors that can occur during database interaction.
/// </summary>
public static class RepositoryErrors
{
    public const string Namespace = "DataIntegration";

    /// <summary>
    /// Represents a situation when an operation executed on a database failed.
    /// </summary>
    public static readonly Error DatabaseOperationFailed = new(
        $"{Namespace}.DatabaseOperationFailed",
        "Saving changes failed.");
}
