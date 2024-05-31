using Microsoft.AspNetCore.Components.Authorization;

namespace BIManagement.Modules.Users.Api.Authorization.Views;

/// <summary>
/// Displays a differing component based the user being or not being Costumer.
/// </summary>
public class AuthorizeCostumerView : AuthorizeView
{
    /// <summary>
    /// Initializes an instance of <see cref="AuthorizeCostumerView"/>.
    /// </summary>
    public AuthorizeCostumerView() : base()
    {
        base.Roles = Domain.Roles.Costumer;
    }
}
