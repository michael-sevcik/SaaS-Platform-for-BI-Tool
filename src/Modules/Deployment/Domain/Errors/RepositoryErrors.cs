using BIManagement.Common.Shared.Errors;

namespace BIManagement.Modules.Deployment.Domain.Errors;

/// <summary>
/// Encapsulates errors that can occur during database interaction.
/// </summary>
public static class RepositoryErrors
{
    public const string Namespace = "Deployment.Repository";

    /// <summary>
    /// Represents a situation when an operation executed on a database failed.
    /// </summary>
    public static readonly Error DatabaseOperationFailed = new(
        $"{Namespace}.DatabaseOperationFailed",
        "Saving changes failed.");

    public static Error EntityNotFound { get; set; } = new(
        $"{Namespace}.EntityNotFound",
        "Entity not found.");
}
