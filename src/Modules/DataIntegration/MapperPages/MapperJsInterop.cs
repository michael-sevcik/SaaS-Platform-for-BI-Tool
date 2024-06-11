using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent;

/// <summary>
/// Represents a functionality of a JavaScript mapping tool.
/// </summary>
internal sealed class MapperJsInterop : IAsyncDisposable
{
    private readonly ILogger logger;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    private Lazy<Task<IJSObjectReference>> mapperObject;

    public MapperJsInterop(IJSRuntime jsRuntime, ILogger<MapperJsInterop> logger)
    {
        this.logger = logger;
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BIManagement.Modules.DataIntegration.MapperComponent/mapperInterop.js").AsTask());

        mapperObject = new(async () => await (await moduleTask.Value).InvokeAsync<IJSObjectReference>("getMappingEditor"));
    }

    // TODO: PROVIDE more reasonable methods
    public async ValueTask<string> GetNumber()
    {
        var module = await moduleTask.Value;
        
        return await module.InvokeAsync<string>("showPrompt", "ahoj");
    }

    public async ValueTask<IJSObjectReference> GetMappingEditor() => await mapperObject.Value;

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
