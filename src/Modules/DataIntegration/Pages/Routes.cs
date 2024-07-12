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
    /// Represents a route to the mapper.
    /// </summary>
    public const string Mapper = $"{DataIntegration}/Mapper";

    /// <summary>
    /// Represents a route to the database schema loading page.
    /// </summary>
    public const string LoadDatabaseSchema= $"{DataIntegration}/LoadDatabaseSchema";

    #region CustomersPages

    /// <summary>
    /// Represents a route to the customer's database configuration.
    /// </summary>
    public const string CustomersConfigureDbConnection = $"{DataIntegration}/Configure-db-connection";

    #endregion

    #region AdminsPages

    /// <summary>
    /// Represents a route to the list of MappingProjects
    /// </summary>
    public const string MappingProjects = $"{DataIntegration}/MappingProjects";

    /// <summary>
    /// Represents a route template for the MappingProjectInfo page.
    /// </summary>
    public const string MappingProjectInfo = $"{MappingProjects}/{{id}}";

    /// <summary>
    /// Gets a route to the page displaying details about a mapping project with a given <paramref name="id"/>.
    /// </summary>
    public static string GetMappingProjectInfo(string id)
        => $"{MappingProjects}/{id}";

    #endregion
}
