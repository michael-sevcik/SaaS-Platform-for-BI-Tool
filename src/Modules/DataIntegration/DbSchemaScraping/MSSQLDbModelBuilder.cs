using BIManagement.Common.Shared.Results;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace BIManagement.Modules.DataIntegration.DbSchemaScraping;

// TODO: Refactor throwing exceptions to using Result<T> instead
/// <summary>
/// Implementation of <see cref="IDbModelBuilder"/> for building a model of a MSSQL database.
/// </summary>
public class MSSQLDbModelBuilder(ILogger<MSSQLDbModelBuilder> logger) : IDbModelBuilder
{
    private static readonly Lazy<ServiceProvider> serviceProvider = new(CreateDesignTimeServiceProvider);

    /// <inheritdoc/>
    public async Task<Result<DbModel>> CreateAsync(DbConnectionConfiguration configuration)
    {
        var modelFactory = CreateEfCoreModelFactory();

        var efCoreDbModel = await Task.Run(() => modelFactory.Create(configuration.ConnectionString, new()));
        if (efCoreDbModel is null)
        {
            return Result.Failure<DbModel>(new(
                $"{Errors.ModelCreationErrorNamespace}.ModelCreationFailed",
                "Reading of database metadata failed."));
        }

        Result<DbModel>? result;
        try
        {
            result = Result.Success(EfCoreModelToDbModel(efCoreDbModel));
        }
        catch (FormatException ex)
        {
            result = Result.Failure<DbModel>(new($"{Errors.ModelCreationErrorNamespace}.FormatError", ex.Message));
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Parsing of EF Core model to DbModel failed due to uknown error.");
            result = Result.Failure<DbModel>(new
                ($"{Errors.ModelCreationErrorNamespace}.ParsingFailedDueToUnknowProblem",
                ex.Message));
        }

        return result;
    }

    private static DbModel EfCoreModelToDbModel(DatabaseModel efCoreModel)
    {
        return new()
        {
            Name = efCoreModel.DatabaseName ?? string.Empty,
            Tables = efCoreModel.Tables.Select(EfCoreTableToTable).ToList()
        };

    }

    private static Domain.DbModelling.Table EfCoreTableToTable(DatabaseTable efCoreTable)
    {
        return new()
        {
            Name = efCoreTable.Name ?? string.Empty,
            Schema = efCoreTable.Schema ?? string.Empty,
            Columns = efCoreTable.Columns.Select(EfCoreColumnToColumn).ToList(),
            PrimaryKeys = efCoreTable.PrimaryKey?.Columns.Select(EfCoreColumnToColumn).ToList() ?? []
        };
    }

    private static Domain.DbModelling.Column EfCoreColumnToColumn(DatabaseColumn efCoreColumn)
    {
        return new()
        {
            Name = efCoreColumn.Name ?? string.Empty,
            DataType = EfCoreDataTypeToDataType(efCoreColumn),
        };
    }

    private static DataTypeBase EfCoreDataTypeToDataType(DatabaseColumn efCoreColumn)
    {
        ArgumentNullException.ThrowIfNull(efCoreColumn.StoreType);
        return efCoreColumn.StoreType switch
        {
            "bit" => new SimpleType(SimpleType.Types.Boolean),
            "tinyint" => new SimpleType(SimpleType.Types.TinyInteger),
            "smallint" => new SimpleType(SimpleType.Types.SmallInteger),
            "int" => new SimpleType(SimpleType.Types.Integer),
            "bigint" => new SimpleType(SimpleType.Types.BigInteger),
            "decimal" => new SimpleType(SimpleType.Types.Decimal),
            "numeric" => new SimpleType(SimpleType.Types.Numeric),
            "smallmoney" => new SimpleType(SimpleType.Types.Money), // TODO: consider creating a separate value for this.
            "money" => new SimpleType(SimpleType.Types.Money),
            string storeType when storeType.StartsWith("float") => new SimpleType(SimpleType.Types.Boolean), // TODO: Consider creating a separate class for this.
            "datetime" => new SimpleType(SimpleType.Types.Datetime),
            "datetimeoffset" => new SimpleType(SimpleType.Types.DatetimeOffset),
            "date" => new SimpleType(SimpleType.Types.Date),
            "time" => new SimpleType(SimpleType.Types.Time),
            "timestamp" => new SimpleType(SimpleType.Types.Timestamp),
            string storeType => CreateComplexTypeOrUnknown(storeType)
        };
    }

    /// <summary>
    /// Parses the store type and creates a new instance of <see cref="DataTypeBase"/> for complex types or unknown types.
    /// </summary>
    /// <param name="storeType">The store type to parse</param>
    /// <returns>Instance of <see cref="DataTypeBase"/>representing the type.</returns>
    /// <exception cref=""></exception>
    private static DataTypeBase CreateComplexTypeOrUnknown(string storeType)
    {

        if (!storeType.StartsWith("nvarchar(") && !storeType.StartsWith("varchar("))
        {
            return new UnknownDataType(storeType);
        }

        if (storeType is "nvarchar(max)" or "varchar(max)")
        {
            return new NVarCharMax();
        }

        var lengthString = storeType[(storeType.IndexOf('(') + 1)..^1];
        if (!int.TryParse(lengthString, out int lenght) || lenght <= 0)
        {
            throw new FormatException($"Cannot parse length from store type: {storeType}");
        }

        return new NVarChar(lenght);
    }



    #region CreationOfEfCoreModelFactory
    // TODO: use classic DI instead of this
    private static ServiceProvider CreateDesignTimeServiceProvider()
    {
        // Source: https://learn.microsoft.com/en-us/ef/core/cli/services
        ServiceCollection services = new();

        // Add the EF Core services to the service collection
        // Source: https://learn.microsoft.com/en-us/ef/core/cli/services
        var assemblyName = "Microsoft.EntityFrameworkCore.SqlServer";
        var providerAssembly = Assembly.Load(assemblyName);
        var providerServicesAttribute = providerAssembly.GetCustomAttribute<DesignTimeProviderServicesAttribute>()
            ?? throw new InvalidOperationException(
                $"Assembly \"{assemblyName}\" is missing required attribute \"{nameof(DesignTimeProviderServicesAttribute)}\"," +
                "which serves for identification of design time service provider.");

        var providerServicesType = providerAssembly.GetType(providerServicesAttribute.TypeName)
            ?? throw new InvalidOperationException($"Assembly \"{assemblyName}\" has to provide type " +
            $"that has \"{nameof(DesignTimeProviderServicesAttribute)}\".");

        var providerServices = (IDesignTimeServices)(Activator.CreateInstance(providerServicesType)
            ?? throw new InvalidOperationException($"EF Core design services provider type \"{providerServicesType.Name}\" is not" +
            $" initializable through \"{nameof(Activator)}\"."));

        providerServices.ConfigureDesignTimeServices(services);

        return services.BuildServiceProvider(); 
    }

    // Its not thread safe and it's scoped
    private static IDatabaseModelFactory CreateEfCoreModelFactory()
        => serviceProvider.Value.GetRequiredService<IDatabaseModelFactory>()
            ?? throw new InvalidOperationException($"Connot create instance of {nameof(IDatabaseModelFactory)}.");
    #endregion
}
