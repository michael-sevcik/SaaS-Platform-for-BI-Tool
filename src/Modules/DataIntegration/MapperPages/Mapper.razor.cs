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
    ICostumerDbModelManager CostumerDbModelManager { get; set; } = default!;

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

    private int testint = 0;
    private bool isInitialized = false;

    private MapperJsInterop? mapperJSInterop;
    private DbModel? costumerDbModel;
    private IReadOnlyList<SchemaMapping>? schemaMappings;
    private IReadOnlyList<TargetDbTable>? targetTables;
    private TargetDbTable targetDbTable = default!;
    private SuccessAlert? successAlert;
    private ErrorAlert? errorAlert;
    private string? Message;

    /// <summary>
    /// The Costumer id to use for the component.
    /// Initialized either by the parent component or by the user id of the current Costumer.
    /// </summary>
    [Parameter]
    public string? CostumerId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (CostumerId == null)
        {
            var userIdResult = await UserAccessor.GetCostumerId(
                HttpContextAccessor.HttpContext ?? throw new InvalidOperationException("Http context is not accesible."));
            if (userIdResult.IsFailure)
            {
                throw new InvalidOperationException(userIdResult.Error.Message);
            }

            CostumerId = userIdResult.Value;
        }

        costumerDbModel = await CostumerDbModelManager.GetAsync(CostumerId);

        schemaMappings = await SchemaMappingRepository.GetSchemaMappings(CostumerId);
        targetTables = await TargetDbTableRepository.GetTargetDbTables();
        if (targetTables.Count <= 0)
        {
            throw new InvalidOperationException("Mapper expects at least one target db table.");
        }

        // pick first table to be mapped
        targetDbTable = targetTables.First();

        isInitialized = true;
        Logger.LogDebug("Mapper component initialized for Costumer with id: {CostumerId}.", CostumerId);
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
            mapperJSInterop = new(JSRuntime, MapperLogger, costumerDbModel
                ?? throw new InvalidOperationException("CostumerDbModel is not initialized.")); // TODO: Exception can be omitted.
            await DisplayMapping();
        }

    }

    private async Task SaveMappingAsync()
    {
        var serializedMapping = await mapperJSInterop!.GetSerializedMappingAsync();
        var isComplete = await mapperJSInterop!.IsMappingCompleteAsync();

        var schemaMapping = new SchemaMapping()
        {
            CostumerId = CostumerId!,
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
            
            Message = result.Error.Message;
            errorAlert.Show();
        }
        else
        {
            if (successAlert is null)
            {
                throw new InvalidOperationException("Error alert is not initialized.");
            }
            
            Message = "Mapping was saved.";
            successAlert.Show();
        }
    }

    private async Task HandleTargetTableSelectionChange()
    {
        Logger.LogDebug("Target table selection changed.");
        targetDbTable = targetTables![testint];
        await DisplayMapping();
    }

    private async ValueTask DisplayMapping()
    {
        var currentMapping = schemaMappings!.Where(mapping => mapping.TargetDbTableId == targetDbTable.Id).FirstOrDefault();
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