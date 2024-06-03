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
    public const string DataIntegration = "/Data-integration";

    /// <summary>
    /// Represents a route to the Data Integration module.
    /// </summary>
    public const string Mapper = $"{DataIntegration}/Mapper";

    /// <summary>
    /// Represents a route to the costumer's database configuration.
    /// </summary>
    public const string ConfigureDbConnction = $"{DataIntegration}/Configure-db-connection";
}
