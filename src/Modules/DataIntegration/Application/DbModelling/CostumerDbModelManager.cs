using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;

namespace BIManagement.Modules.DataIntegration.Application.DbModelling;

/// <summary>
/// Default implementation of <see cref="ICostumerDbModelManager"/>.
/// </summary>
internal class CostumerDbModelManager(IDbModelBuilderAccessor modelBuilderAccessor, ICostumerDbModelRepository costumerDbModelRepository)
    : ICostumerDbModelManager, IScoped
{
    /// <inheritdoc/>
    public async Task<Result<DbModel>> CreateDbModelAsync(CostumerDbConnectionConfiguration configuration)
    {
        IDbModelFactory modelBuilder;
        switch (configuration.Provider)
        {
            case DatabaseProvider.SqlServer:
                modelBuilder = modelBuilderAccessor.GetMSSQLDbModelBuilder(); // TODO: REGISTER THIS SERVICE
                break;
            default:
                throw new NotImplementedException();
        }

        var modelResult = await modelBuilder.CreateAsync(configuration);
        if (modelResult.IsFailure)
        {
            return modelResult;
        }

        var additionResult = await costumerDbModelRepository.AddAsync(new() {
            CostumerId = configuration.CostumerId,
            DbModel = modelResult.Value
        });

        return additionResult.Map(() => modelResult.Value);
    }

    /// <inheritdoc/>
    public async Task<DbModel?> GetAsync(string costumerId)
        => (await costumerDbModelRepository.GetAsync(costumerId))?.DbModel;
}
