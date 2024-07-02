using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Api;
using BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;
using BIManagement.Modules.DataIntegration.Application.Mapping.SqlViewGenerating;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel;
using System.Text.Json;

namespace BIManagement.Modules.DataIntegration.Application;

/// <summary>
/// Represents a service for data integration.
/// Default implementation of <see cref="IDataIntegrationService"/>.
/// </summary>
/// <param name="schemaMappingRepository">The schema mapping repository</param>
/// <param name="dbConnectionConfigurationRepository">Repository for db connection configurations.</param>
internal class DataIntegrationService(
    ISchemaMappingRepository schemaMappingRepository,
    ICustomerDbConnectionConfigurationRepository dbConnectionConfigurationRepository) : IDataIntegrationService, ITransient
{
    /// <inheritdoc />
    public async Task<Result<string[]>> GenerateSqlViewsForCustomer(string customerId)
    {
        List<string> views = new();
        foreach (var sm in await schemaMappingRepository.GetSchemaMappings(customerId))
        {
            var entityMapping = JsonSerializer.Deserialize<EntityMapping>(sm.Mapping, MappingJsonOptions.CreateOptions());
            if (entityMapping is null)
            {
                return Result.Failure<string[]>(new(
                    "DataIntegration.GeneratingSQLView.ParsingFailed",
                    "Parsing of SQL view failed."));
            }

            var sqlView = EntityMappingViewGenerator.GenerateSqlView(entityMapping);
            views.Add(sqlView);
        }

        return Result.Success(views.ToArray());
    }

    private static Api.DatabaseProvider MapDatabaseProviders(Domain.DatabaseConnection.DatabaseProvider databaseProvider)
    {
        return databaseProvider switch
        {
            Domain.DatabaseConnection.DatabaseProvider.SqlServer => Api.DatabaseProvider.SqlServer,
            _ => throw new NotImplementedException()
        };
    }

    /// <inheritdoc />
    public async Task<(Api.DatabaseProvider, string)?> GetCustomerDbConnectionString(string customerId)
    {
        var dbConf = await dbConnectionConfigurationRepository.GetAsync(customerId);
        return dbConf is null ? null : (DataIntegrationService.MapDatabaseProviders(dbConf.Provider), dbConf.ConnectionString);
    }
}
