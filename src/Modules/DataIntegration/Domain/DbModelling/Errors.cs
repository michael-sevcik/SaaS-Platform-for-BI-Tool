using BIManagement.Common.Shared.Errors;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Encapsulates errors that can occur during database modeling.
/// </summary>
public static class Errors
{
    /// <summary>
    /// Represents a namespace for errors that occur during database modeling.
    /// </summary>
    public const string ModelCreationErrorNamespace = "DataIntegration.DbModeling.";

    /// <summary>
    /// Represents failed attempt to connect to the modeled database.
    /// </summary>
    public static readonly Error ConnectionFailed = new($"{ModelCreationErrorNamespace}.ConnectionFailed", "Failed to connect to the database.");

    /// <summary>
    /// Represents situation when the user has insufficient privileges to create a database model.
    /// </summary>
    public static readonly Error InsufficentPrivilages = new(
        $"{ModelCreationErrorNamespace}.InsufficentPrivilages",
        "Granted privilages are insufficent to create a database model.");
}
