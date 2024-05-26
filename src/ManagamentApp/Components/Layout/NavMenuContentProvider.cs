using BIManagement.Common.Components.Layout;
using Microsoft.AspNetCore.Components;

namespace BIManagement.ManagementApp.Components.Layout;

public class NavMenuContentProvider : INavMenuContentProvider
{
    public RenderFragment Content => NavMenu.MenuContent;
}
