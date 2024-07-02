using Microsoft.AspNetCore.Components.Authorization;

namespace BIManagement.Modules.Users.Api.Authorization.Views;

/// <summary>
/// Displays a differing component based the user being or not being Costumer.
/// </summary>
public class AuthorizeCustomerView : AuthorizeView
{
    /// <summary>
    /// Initializes an instance of <see cref="AuthorizeCustomerView"/>.
    /// </summary>
    public AuthorizeCustomerView() : base()
    {
        base.Roles = Domain.Roles.Customer;
    }
}
