using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Application.DatabaseConnection;

internal class DatabaseConnectionConfigurationManager : IDatabaseConnectionConfigurationRepository, IScoped
{
    public Task DeleteAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<DatabaseConnectionConfiguration?> GetAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(string userId, DatabaseConnectionConfiguration configuration)
    {
        throw new NotImplementedException();
    }
}
