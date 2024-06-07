using BIManagement.Common.Shared.Errors;

namespace BIManagement.Modules.DataIntegration.Domain.DbModelling;

/// <summary>
/// Encapsulates errors that can occur during database modeling.
/// </summary>
public static class Errors
{
    /// <summary>
    /// Represents failed attempt to connect to the modeled database.
    /// </summary>
    public static readonly Error ConnectionFailed = new("DataIntegration.DbModeling.ConnectionFailed", "Failed to connect to the database.");

    /// <summary>
    /// Represents situation when the user has insufficient privileges to create a database model.
    /// </summary>
    public static readonly Error InsufficentPrivilages = new(
        "DataIntegration.DbModeling.InsufficentPrivilages",
        "Granted privilages are insufficent to create a database model.");
}
