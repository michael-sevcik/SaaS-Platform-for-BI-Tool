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
    /// <summary>
    /// Represents a base route for the Data Integration module.
    /// </summary>
    public const string Users = "/Users";

    /// <summary>
    /// Represents a route for the Costumers page.
    /// </summary>
    public const string Costumers = $"{Users}/costumers";

    /// <summary>
    /// Represents a route for the Admins page.
    /// </summary>
    public const string Admins = $"{Users}/admins";


    public const string CostumerInfo = $"{Costumers}/{{Id}}";

    public const string AdminInfo = $"{Admins}/{{Id}}";

    public static string GetCostumerInfo(string id)
        => $"{Costumers}/{id}";

    public static string GetAdminInfo(string id)
    => $"{Admins}/{id}";

    public const string AddCostumer = $"{Costumers}/add";
    public const string AddAdmin = $"{Admins}/add";
}
