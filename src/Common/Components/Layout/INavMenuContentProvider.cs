using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Common.Components.Layout
{
    public interface INavMenuContentProvider
    {
        RenderFragment Content { get; }
    }
}
