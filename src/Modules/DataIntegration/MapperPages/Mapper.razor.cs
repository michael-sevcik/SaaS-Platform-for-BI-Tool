using BIManagement.Common.Components.InteractiveAlerts;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using BIManagement.Modules.Users.Api;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent;

public sealed partial class Mapper : IAsyncDisposable
{
    [Inject]
    IJSRuntime JSRuntime { get; set; } = default!;

    [Inject]
    ICustomerDbModelManager CustomerDbModelManager { get; set; } = default!;

    [Inject]
    IUserAccessor UserAccessor { get; set; } = default!;

    [Inject]
    IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    [Inject]
    ILogger<Mapper> Logger { get; set; } = default!;

    [Inject]
    ILogger<MapperJsInterop> MapperLogger { get; set; } = default!;

    [Inject]
    ISchemaMappingRepository SchemaMappingRepository { get; set; } = default!;

    [Inject]
    ITargetDbTableRepository TargetDbTableRepository { get; set; } = default!;

    private const string IntroductionMessage = $"""
        Please map the target entities one by one, don't forget to save your progress after each entity mapped.
        Fully mapped entities are marked with {FullyMappedSymbol}, othrerwise {UnfinishedSymbol}.
        After mapping all entities and saving each mapping, Continu button will appear.
        """;
    private bool isInitialized = false;

    private MapperJsInterop? mapperJSInterop;
    private int currentTargetTableIndex = 0;
    private DbModel? customerDbModel;
    private IReadOnlyList<TargetDbTable>? targetTables;
    private bool[]? mappingStates;
    private TargetDbTable? targetDbTable;
    private SuccessAlert? successAlert;
    private ErrorAlert? errorAlert;
    private string? message;

    /// <summary>
    /// The Customer id to use for the component.
    /// Initialized either by the parent component or by the user id of the current Customer.
    /// </summary>
    [Parameter]
    public string? CustomerId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (CustomerId == null)
        {
            var userIdResult = await UserAccessor.GetCustomerId(
                HttpContextAccessor.HttpContext ?? throw new InvalidOperationException("Http context is not accesible."));
            if (userIdResult.IsFailure)
            {
                throw new InvalidOperationException(userIdResult.Error.Message);
            }

            CustomerId = userIdResult.Value;
        }

        customerDbModel = await CustomerDbModelManager.GetAsync(CustomerId);
        if (customerDbModel is null)
        {
            message = "You have to create a database model first. Use Database connection configuration for that";
            errorAlert?.Show();
            return;
        }

        targetTables = await TargetDbTableRepository.GetTargetDbTables();
        if (targetTables.Count <= 0)
        {
            throw new InvalidOperationException("Mapper expects at least one target db table.");
        }

        var schemaMappingsByTargetTableId = (await SchemaMappingRepository.GetSchemaMappings(CustomerId))
            .ToDictionary(m => m.TargetDbTableId);
        mappingStates = targetTables.Select(table =>
        {
            if (!schemaMappingsByTargetTableId.TryGetValue(table.Id, out var mapping))
            {
                return false;
            }

            return mapping.IsComplete;
        }).ToArray();

        targetDbTable = targetTables.First();

        message = IntroductionMessage;
        successAlert?.Show();
        isInitialized = true;
        Logger.LogDebug("Mapper component initialized for Customer with id: {customerId}.", CustomerId);
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool isFirst)
    {
        Logger.LogDebug(
            "Mapper component rendered. Is rendered for the first time: {isFirstRender}, mapperJSInterop {mapperJSInterop}, JsRuntime {JsRuntime}",
            isFirst,
            mapperJSInterop,
            JSRuntime);

        if (!isInitialized)
        {
            return;
        }

        if (mapperJSInterop is null)
        {
            mapperJSInterop = new(JSRuntime, MapperLogger, customerDbModel
                ?? throw new InvalidOperationException("CustomerDbModel is not initialized.")); // TODO: Exception can be omitted.
            await DisplayMapping();
        }

    }

    private async Task SaveMappingAsync()
    {
        if (targetDbTable is null)
        {
            return;
        }

        var serializedMapping = await mapperJSInterop!.GetSerializedMappingAsync();
        var isComplete = await mapperJSInterop!.IsMappingCompleteAsync();

        var schemaMapping = new SchemaMapping()
        {
            CustomerId = CustomerId!,
            TargetDbTableId = targetDbTable.Id,
            Mapping = serializedMapping,
            IsComplete = isComplete,
        };

        var result = await SchemaMappingRepository.SaveAsync(schemaMapping);
        if (result.IsFailure)
        {
            if (errorAlert is null)
            {
                throw new InvalidOperationException("Error alert is not initialized.");
            }
            
            message = result.Error.Message;
            errorAlert.Show();
        }
        else
        {
            if (successAlert is null)
            {
                throw new InvalidOperationException("Error alert is not initialized.");
            }

            mappingStates![currentTargetTableIndex] = isComplete;
            message = "Mapping was saved.";
            successAlert.Show();
        }
    }

    private async Task HandleTargetTableSelectionChange()
    {
        Logger.LogDebug("Target table selection changed.");
        targetDbTable = targetTables![currentTargetTableIndex];
        await DisplayMapping();
    }

    private async ValueTask DisplayMapping()
    {
        if (CustomerId is null)
        {
            Logger.LogWarning("DisplayMapping called with null CustomerId.");
            return;
        }

        var currentMapping = await SchemaMappingRepository.GetSchemaMapping(CustomerId, targetDbTable.Id);
        if (currentMapping is not null)
        {
            await mapperJSInterop!.LoadEntityMapping(currentMapping.Mapping, targetDbTable);
        }
        else
        {
            await mapperJSInterop!.InitializeMapperWithTargetTableAsync(targetDbTable);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (mapperJSInterop is not null)
            {
                Logger.LogDebug("MapperJSInterop is being disposed.");
                await mapperJSInterop.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // since we are disposing, we can ignore this exception
        }
    }

}