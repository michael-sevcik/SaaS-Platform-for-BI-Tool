using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.DbSchemaScraping;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Application.DbModelling;

/// <summary>
/// Default implementation of <see cref="ICostumerDbModelManager"/>.
/// </summary>
internal class CostumerDbModelManager(IServiceProvider serviceProvider, ICostumerDbModelRepository costumerDbModelRepository)
    : ICostumerDbModelManager, IScoped
{
    /// <inheritdoc/>
    public async Task<Result<DbModel>> CreateDbModelAsync(CostumerDbConnectionConfiguration configuration)
    {
        IDbModelBuilder modelBuilder;
        switch (configuration.Provider)
        {
            case DatabaseProvider.SqlServer:
                modelBuilder = serviceProvider.GetRequiredService<MSSQLDbModelBuilder>(); // TODO: REGISTER THIS SERVICE
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
