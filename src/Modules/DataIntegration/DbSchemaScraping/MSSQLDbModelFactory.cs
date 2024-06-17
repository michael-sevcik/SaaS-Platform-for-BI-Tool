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
/// Implementation of <see cref="IDbModelFactory"/> for building a model of a MSSQL database.
/// </summary>
public class MSSQLDbModelFactory(ILogger<MSSQLDbModelFactory> logger) : IDbModelFactory
{
    private static readonly Lazy<ServiceProvider> serviceProvider = new(CreateDesignTimeServiceProvider);

    /// <inheritdoc/>
    public async Task<Result<DbModel>> CreateAsync(DbConnectionConfiguration configuration)
    {
        var modelFactory = CreateEfCoreModelFactory();

        DatabaseModel? efCoreDbModel;

        try
        {
            efCoreDbModel = await Task.Run(() => modelFactory.Create(configuration.ConnectionString, new()));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Reading of database metadata failed.");
            return Result.Failure<DbModel>(new(
                $"{Errors.ModelCreationErrorNamespace}.ModelCreationFailed",
                "Reading of database metadata failed."));
        }
        

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

    private DbModel EfCoreModelToDbModel(DatabaseModel efCoreModel)
    {
        return new()
        {
            Name = efCoreModel.DatabaseName ?? string.Empty,
            Tables = efCoreModel.Tables.Select(EfCoreTableToTable).ToList()
        };

    }

    private Domain.DbModelling.Table EfCoreTableToTable(DatabaseTable efCoreTable)
    {
        return new()
        {
            Name = efCoreTable.Name ?? string.Empty,
            Schema = efCoreTable.Schema ?? string.Empty,
            Columns = efCoreTable.Columns.Select(EfCoreColumnToColumn).ToList(),
            PrimaryKeys = efCoreTable.PrimaryKey?.Columns.Select(EfCoreColumnToColumn).ToList() ?? []
        };
    }

    private Domain.DbModelling.Column EfCoreColumnToColumn(DatabaseColumn efCoreColumn)
    {
        return new()
        {
            Name = efCoreColumn.Name ?? string.Empty,
            DataType = EfCoreDataTypeToDataType(efCoreColumn),
        };
    }

    private DataTypeBase EfCoreDataTypeToDataType(DatabaseColumn efCoreColumn)
    {
        ArgumentNullException.ThrowIfNull(efCoreColumn.StoreType);
        bool isNullable = efCoreColumn.IsNullable;
        return efCoreColumn.StoreType switch
        {
            "bit" => new SimpleType(SimpleType.Types.Boolean, isNullable),
            "tinyint" => new SimpleType(SimpleType.Types.TinyInteger, isNullable),
            "smallint" => new SimpleType(SimpleType.Types.SmallInteger, isNullable),
            "int" => new SimpleType(SimpleType.Types.Integer, isNullable),
            "bigint" => new SimpleType(SimpleType.Types.BigInteger, isNullable),
            "decimal" => new SimpleType(SimpleType.Types.Decimal, isNullable),
            "numeric" => new SimpleType(SimpleType.Types.Numeric, isNullable),
            "smallmoney" => new SimpleType(SimpleType.Types.Money, isNullable), // TODO: consider creating a separate value for this.
            "money" => new SimpleType(SimpleType.Types.Money, isNullable),
            string storeType when storeType.StartsWith("float") => new SimpleType(SimpleType.Types.Float, isNullable), // TODO: Consider creating a separate class for this.
            string storeType when storeType.StartsWith("numeric") => new SimpleType(SimpleType.Types.Numeric, isNullable), // TODO: Consider creating a separate class for this.
            string storeType when storeType.StartsWith("decimal") => new SimpleType(SimpleType.Types.Decimal, isNullable), // TODO: Consider creating a separate class for this.
            "datetime" => new SimpleType(SimpleType.Types.Datetime, isNullable),
            "datetime2" => new SimpleType(SimpleType.Types.Datetime, isNullable),
            "datetimeoffset" => new SimpleType(SimpleType.Types.DatetimeOffset, isNullable),
            "date" => new SimpleType(SimpleType.Types.Date, isNullable),
            "time" => new SimpleType(SimpleType.Types.Time, isNullable),
            "timestamp" => new SimpleType(SimpleType.Types.Timestamp, isNullable),
            string storeType => CreateComplexTypeOrUnknown(storeType, isNullable)
        };
    }

    /// <summary>
    /// Parses the store type and creates a new instance of <see cref="DataTypeBase"/> for complex types or unknown types.
    /// </summary>
    /// <param name="storeType">The store type to parse</param>
    /// <param name="isNullable">Indicates whether the data type should be nullable</param>
    /// <returns>Instance of <see cref="DataTypeBase"/>representing the type.</returns>
    /// <exception cref=""></exception>
    private DataTypeBase CreateComplexTypeOrUnknown(string storeType, bool isNullable)
    {

        if (!storeType.StartsWith("nvarchar(") && !storeType.StartsWith("varchar("))
        {
            logger.LogWarning("Unknown data type: {storeType}", storeType);
            return new UnknownDataType(storeType, isNullable);
        }

        if (storeType is "nvarchar(max)" or "varchar(max)")
        {
            return new NVarCharMax(isNullable);
        }

        var lengthString = storeType[(storeType.IndexOf('(') + 1)..^1];
        if (!int.TryParse(lengthString, out int lenght) || lenght <= 0)
        {
            throw new FormatException($"Cannot parse length from store type: {storeType}");
        }

        return new NVarChar(lenght, isNullable);
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
