using Microsoft.AspNetCore.Components;

namespace BIManagement.Common.Components.Layout;

/// <summary>
/// Represents a provider of the content for the <see cref="NavMenu"/> component.
/// </summary>
public interface INavMenuContentProvider
{
    /// <summary>
    /// Gets the content for the <see cref="NavMenu"/> component.
    /// </summary>
    RenderFragment Content { get; }
}
