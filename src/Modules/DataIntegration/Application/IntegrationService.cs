using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Api;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping;

namespace BIManagement.Modules.DataIntegration.Application;


/// <summary>
/// Default implementation of <see cref="IIntegrationService"/>.
/// </summary>
/// <param name="schemaMappingRepository">The repository for managing schema mappings.</param>
/// <param name="dbConnectionConfigurationRepository">The repository for managing database connection configurations.</param>
/// <param name="customerDbModelRepository">The repository for managing customer database models.</param>
public class IntegrationService(
    ISchemaMappingRepository schemaMappingRepository,
    ICustomerDbConnectionConfigurationRepository dbConnectionConfigurationRepository,
    ICustomerDbModelRepository customerDbModelRepository) : IIntegrationService, IScoped
{
    /// <inheritdoc/>
    public async Task HandleCustomerDeletionAsync(string userId)
    {
        await schemaMappingRepository.DeleteSchemaMappings(userId);
        await dbConnectionConfigurationRepository.DeleteAsync(userId);
        await customerDbModelRepository.DeleteAsync(userId);
    }
}
