using BIManagement.Modules.DataIntegration.Domain.DbModelling;

namespace BIManagement.Modules.DataIntegration.Application.DbModelling;

/// <summary>
/// Represents an accessor for getting a <see cref="IDbModelFactory"/> for various DB providers.
/// </summary>
public interface IDbModelBuilderAccessor
{
    /// <summary>
    /// Gets a <see cref="IDbModelFactory"/> for building a model of a MSSQL database.
    /// </summary>
    /// <returns>Instance of <see cref="IDbModelFactory"/> for building a model of a MSSQL database.</returns>
    IDbModelFactory GetMSSQLDbModelBuilder();
}
