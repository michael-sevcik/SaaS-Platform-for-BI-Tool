using BIManagement.Modules.DataIntegration.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Persistence;

internal class DataIntegrationDbContext : DbContext
{
    public DbSet<DatabaseConnectionConfiguration> DatabaseConnectionConfigurations { get; set; }
    public DataIntegrationDbContext(DbContextOptions<DataIntegrationDbContext> options) : base(options)
    {
    }

}
