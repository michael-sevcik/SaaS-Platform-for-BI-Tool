using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent;

/// <summary>
/// Represents a functionality of a JavaScript mapping tool.
/// </summary>
internal sealed class MapperJsInterop(IJSRuntime jsRuntime, ILogger<MapperJsInterop> logger) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BIManagement.Modules.DataIntegration.MapperComponent/mapperInterop.js").AsTask());

    private IJSObjectReference? MapperObject;

   
    // TODO: PROVIDE more reasonable methods
    public async ValueTask<string> GetNumber()
    {
        var module = await moduleTask.Value;

        return await module.InvokeAsync<string>("showPrompt", "ahoj");
    }

    public async ValueTask<IJSObjectReference> GetMappingEditor()
    {
        var module = await moduleTask.Value;
        logger.LogDebug("MapperJsInterop getMappingEditor invoked.");
        return await module.InvokeAsync<IJSObjectReference>("getMappingEditor");
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
            logger.LogDebug("MapperJsInterop disposed.");
        }
    }
}
