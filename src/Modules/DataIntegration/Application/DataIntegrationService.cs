using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Api;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Application;

internal class DataIntegrationService(
    ISchemaMappingRepository schemaMappingRepository,
    ICustomerDbConnectionConfigurationRepository dbConnectionConfigurationRepository) : IDataIntegrationService, IScoped
{
    public Task<Result<string[]>> GenerateSqlViewsForCustomer(string customerId)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetCustomerDbConnectionString(string customerId)
    {
        throw new NotImplementedException();
    }
}
