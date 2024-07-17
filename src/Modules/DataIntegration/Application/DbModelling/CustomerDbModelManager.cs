using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;

namespace BIManagement.Modules.DataIntegration.Application.DbModelling;

/// <summary>
/// Default implementation of <see cref="ICustomerDbModelManager"/>.
/// </summary>
internal class CustomerDbModelManager(IDbModelBuilderAccessor modelBuilderAccessor, ICustomerDbModelRepository customerDbModelRepository)
    : ICustomerDbModelManager, IScoped
{
    /// <inheritdoc/>
    public async Task<Result<DbModel>> CreateDbModelAsync(CustomerDbConnectionConfiguration configuration)
    {
        IDbModelFactory modelBuilder;
        switch (configuration.Provider)
        {
            case DatabaseProvider.SqlServer:
                modelBuilder = modelBuilderAccessor.GetMSSQLDbModelBuilder();
                break;
            default:
                throw new NotImplementedException();
        }

        var modelResult = await modelBuilder.CreateAsync(configuration);
        if (modelResult.IsFailure)
        {
            return modelResult;
        }

        var additionResult = await customerDbModelRepository.SaveAsync(new() {
            CustomerId = configuration.CustomerId,
            DbModel = modelResult.Value
        });

        return additionResult.Map(() => modelResult.Value);
    }

    /// <inheritdoc/>
    public async Task<DbModel?> GetAsync(string customerId)
        => (await customerDbModelRepository.GetAsync(customerId))?.DbModel;
}
