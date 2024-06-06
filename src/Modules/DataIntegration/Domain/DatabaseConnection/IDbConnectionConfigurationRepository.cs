using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

public interface IDbConnectionConfigurationRepository
{
    // TODO: Consider using result to pass potential errors
    Task SaveAsync(string userId, DbConnectionConfiguration configuration);
    Task DeleteAsync(string userId);
    Task<DbConnectionConfiguration?> GetAsync(string userId);
}
