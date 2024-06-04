using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

public interface IDatabaseConnectionConfigurationManager
{
    Task SaveAsync(string userId, DatabaseConnectionConfiguration configuration);
    Task DeleteAsync(string userId);
    Task<DatabaseConnectionConfiguration?> GetAsync(string userId);
}
