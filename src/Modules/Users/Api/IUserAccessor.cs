using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Users.Api;

/// <summary>
/// Represents helper class that provides access to the current user.
/// </summary>
public interface IUserAccessor
{
    /// <summary>
    /// Gets the email of the current user.
    /// </summary>
    /// <param name="context">Context of the HTTP request which contains the user principal.</param>
    /// <returns>Task object representing the asynchronous operation.</returns>
    Task<string?> GetUserEmailAsync(HttpContext context);

    /// <summary>
    /// Gets the Id of the current user.
    /// </summary>
    /// <param name="context">Context of the HTTP request which contains the user principal.</param>
    /// <returns>Task object representing the asynchronous operation.</returns>
    Task<string?> GetUserIdAsync(HttpContext context);

    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <param name="context">Context of the HTTP request which contains the user principal.</param>
    /// <returns>Task object representing the asynchronous operation.</returns>
    Task<string?> GetUserNameAsync(HttpContext context);
}
