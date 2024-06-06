using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.DbSchemaScraping;

/// <summary>
/// Implementation of <see cref="IDbModelBuilder"/> for building a model of a MSSQL database.
/// </summary>
public class MSSQLDbModelBuilder : IDbModelBuilder
{
    /// <inheritdoc/>
    public Task<Result<DbModel>> CreateAsync(DbConnectionConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
