using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Users.Pages;

/// <summary>
/// Represents an encapsulation of routes for the Users module.
/// </summary>
internal static class Routes
{
    #region AdminsPages
 
    /// <summary>
    /// Represents a base route for the Data Integration module.
    /// </summary>
    public const string UsersGroup = "/Users";

    /// <summary>
    /// Represents a route to the Costumers page.
    /// </summary>
    public const string Costumers = $"{UsersGroup}/costumers";

    /// <summary>
    /// Represents a route to the Admins page.
    /// </summary>
    public const string Admins = $"{UsersGroup}/admins";

    /// <summary>
    /// Represents a route template for the Admins page.
    /// </summary>
    public const string CostumerInfo = $"{Costumers}/{{Id}}";

    /// <summary>
    /// Represents a route template for the Admins page.
    /// </summary>
    public const string AdminInfo = $"{Admins}/{{Id}}";

    /// <summary>
    /// Gets a route to the page displaying details about a costumer.
    /// </summary>
    public static string GetCostumerInfo(string id)
        => $"{Costumers}/{id}";

    /// <summary>
    /// Gets a route to the page displaying details about admin.
    /// </summary>
    public static string GetAdminInfo(string id)
    => $"{Admins}/{id}";

    /// <summary>
    /// Represents a route to the the page for adding costumer's accounts.
    /// </summary>
    public const string AddCostumer = $"{Costumers}/add";

    /// <summary>
    /// Represents a route to the page for adding admin users.
    /// </summary>
    public const string AddAdmin = $"{Admins}/add";

    #endregion
}
