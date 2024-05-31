using Microsoft.AspNetCore.Components.Authorization;

namespace BIManagement.Modules.Users.Api.Authorization.Views;

/// <summary>
/// Displays a differing component based the user being or not being admin.
/// </summary>
public class AuthorizeAdminView : AuthorizeView
{
    /// <summary>
    /// Initializes an instance of <see cref="AuthorizeAdminView"/>.
    /// </summary>
    public AuthorizeAdminView() : base()
    {
        base.Roles = Domain.Roles.Admin;
    }
}
