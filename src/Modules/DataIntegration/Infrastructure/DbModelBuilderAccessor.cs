using BIManagement.Modules.DataIntegration.Application.DbModelling;
using BIManagement.Modules.DataIntegration.DbSchemaScraping;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BIManagement.Modules.DataIntegration.Infrastructure;

internal class DbModelBuilderAccessor(IServiceProvider serviceProvider) : IDbModelBuilderAccessor
{
    public IDbModelFactory GetMSSQLDbModelBuilder()
    {
        var logger = serviceProvider.GetRequiredService<ILogger<MSSQLDbModelFactory>>();
        return new MSSQLDbModelFactory(logger);
    }
}
