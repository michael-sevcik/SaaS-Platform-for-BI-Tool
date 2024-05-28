using BIManagement.Common.Components.Layout;
using Microsoft.AspNetCore.Components;

namespace BIManagement.ManagementApp.Components.Layout;

/// <summary>
/// Represents a provider of the navigation menu content.
/// </summary>
public class NavMenuContentProvider : INavMenuContentProvider
{
    /// <inheritdoc/>
    public RenderFragment Content => NavMenuContent.MenuContent;
}
