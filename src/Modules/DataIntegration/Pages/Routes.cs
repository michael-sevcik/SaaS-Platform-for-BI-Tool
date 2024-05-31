using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Pages;

/// <summary>
/// Represents an encapsulation of routes for the Data Integration module.
/// </summary>
internal static class Routes
{
    /// <summary>
    /// Represents a base route for the Data Integration module.
    /// </summary>
    public const string DataIntegration = "/data-integration";

    /// <summary>
    /// Represents a route for the Data Integration module.
    /// </summary>
    public const string Mapper = $"{DataIntegration}/mapper";
}
