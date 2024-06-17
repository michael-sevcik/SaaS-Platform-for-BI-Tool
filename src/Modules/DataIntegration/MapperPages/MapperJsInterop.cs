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

    /// <summary>
    /// Initializes a new instance of the <see cref="MapperJsInterop"/> class.
    /// </summary>
    /// <param name="jsRuntime">The JavaScript runtime.</param>
    /// <param name="logger">The logger.</param>
    /// <param name="sourceDbModel">The source database model.</param>
    public MapperJsInterop(IJSRuntime jsRuntime, ILogger<MapperJsInterop> logger, DbModel sourceDbModel)
    {
        this.logger = logger;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BIManagement.Modules.DataIntegration.MapperComponent/mapperJsInteropBridge.js").AsTask());

        var serializedSourceDbModel = JsonSerializer.Serialize(sourceDbModel, SerializationOptions.Default);
        mapperObject = new(async () => await (await moduleTask.Value).InvokeAsync<IJSObjectReference>("getMappingEditor", serializedSourceDbModel));
    }

    /// <summary>
    /// Gets the mapping editor.
    /// </summary>
    /// <returns>The mapping editor.</returns>
    public async ValueTask<IJSObjectReference> GetMappingEditorAsync() => await mapperObject.Value;

    /// <summary>
    /// Initializes the mapper with the target table.
    /// </summary>
    /// <param name="targetTable">The target database table.</param>
    public async ValueTask InitializeMapperWithTargetTableAsync(TargetDbTable targetTable)
    {
        var mapper = await mapperObject.Value;
        var serializedTableModel = JsonSerializer.Serialize(targetTable.TableModel, SerializationOptions.Default);
        await mapper.InvokeVoidAsync("createFromSerializedTargetTable", serializedTableModel);
    }

    /// <summary>
    /// Loads the entity mapping.
    /// </summary>
    /// <param name="serializedEntityMapping">The serialized entity mapping.</param>
    /// <param name="targetTable">The target database table.</param>
    public async ValueTask LoadEntityMapping(string serializedEntityMapping, TargetDbTable targetTable)
    {
        var mapper = await mapperObject.Value;
        var serializedTableModel = JsonSerializer.Serialize(targetTable.TableModel, SerializationOptions.Default);
        await mapper.InvokeVoidAsync("loadSerializedEntityMapping", serializedEntityMapping, serializedTableModel);
    }

    /// <summary>
    /// Asynchronously checks if the mapping is complete.
    /// </summary>
    /// <returns><c>true</c> if the mapping is complete; otherwise, <c>false</c>.</returns>
    public async ValueTask<bool> IsMappingCompleteAsync()
    {
        var mapper = await mapperObject.Value;
        return await mapper.InvokeAsync<bool>("isMappingComplete");
    }

    /// <summary>
    /// Asynchronously gets the serialized mapping.
    /// </summary>
    /// <returns>The serialized mapping.</returns>
    public async ValueTask<string> GetSerializedMappingAsync()
    {
        var mapper = await mapperObject.Value;
        return await mapper.InvokeAsync<string>("createSerializedMapping");
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
