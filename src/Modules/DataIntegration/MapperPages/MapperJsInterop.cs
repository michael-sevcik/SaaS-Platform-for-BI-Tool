using Microsoft.JSInterop;

namespace BIManagement.Modules.DataIntegration.MapperComponent;

/// <summary>
/// Represents a functionality of a JavaScript mapping tool.
/// </summary>
internal sealed class MapperJsInterop(IJSRuntime jsRuntime) : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/BIManagement.Modules.DataIntegration.MapperComponent/mapperInterop.js").AsTask());

    // TODO: PROVIDE more reasonable methods
    public async ValueTask<string> GetNumber()
    {
        var module = await moduleTask.Value;

        return await module.InvokeAsync<string>("showPrompt", "ahoj");
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
