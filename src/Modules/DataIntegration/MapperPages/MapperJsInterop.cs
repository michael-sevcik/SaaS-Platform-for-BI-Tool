using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BIManagement.Modules.DataIntegration.MapperComponent;

/// <summary>
/// Represents a functionality of a JavaScript mapping tool.
/// </summary>
internal sealed class MapperJsInterop : IAsyncDisposable
{
    private readonly ILogger logger;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    private Lazy<Task<IJSObjectReference>> mapperObject;

    public MapperJsInterop(IJSRuntime jsRuntime, ILogger<MapperJsInterop> logger, DbModel sourceDbModel)
    {
        this.logger = logger;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BIManagement.Modules.DataIntegration.MapperComponent/mapperInterop.js").AsTask());

        var serializedSourceDbModel = JsonSerializer.Serialize(sourceDbModel, SerializationOptions.Default);
        mapperObject = new(async () => await (await moduleTask.Value).InvokeAsync<IJSObjectReference>("getMappingEditor", serializedSourceDbModel));
    }

    // TODO: PROVIDE more reasonable methods
    public async ValueTask<string> GetNumber()
    {
        var module = await moduleTask.Value;
        
        return await module.InvokeAsync<string>("showPrompt", "ahoj");
    }

    public async ValueTask<IJSObjectReference> GetMappingEditor() => await mapperObject.Value;

    public async ValueTask InitializeMapperWithTargetTable(TargetDbTable targetTable)
    {
        var mapper = await mapperObject.Value;
        var serializedTableModel = JsonSerializer.Serialize(targetTable.TableModel, SerializationOptions.Default);
        await mapper.InvokeVoidAsync("createFromSerializedTargetTable", serializedTableModel);
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (mapperObject.IsValueCreated)
        {
            logger.LogDebug("MapperObject disposed.");
            var value = await mapperObject.Value;
            await value.DisposeAsync();
        }

        if (moduleTask.IsValueCreated)
        {
            logger.LogDebug("MapperJsInterop disposed.");
            var value = await moduleTask.Value;
            await value.DisposeAsync();
        }
    }
}
