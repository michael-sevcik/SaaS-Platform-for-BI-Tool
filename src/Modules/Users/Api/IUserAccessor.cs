using BIManagement.Common.Shared.Errors;
using BIManagement.Common.Shared.Results;
using Microsoft.AspNetCore.Http;

namespace BIManagement.Modules.Users.Api;

/// <summary>
/// Represents helper class that provides access to the current user.
/// </summary>
public interface IUserAccessor
{
    /// <summary>
    /// This error signals that the current user is not a customer.
    /// </summary>
    public static readonly Error NotCustomerError = new(
        "Users.UserIsNotCustomer",
        "Provided user is not a customer.");

    /// <summary>
    /// This error signals that HttpContext does not have a user principal
    /// or the user could not been identified based on that principal.
    /// </summary>
    public static readonly Error UserNotIdentifiableError = new(
        "Users.UserNotIdentifiable",
        $"Cannot identify user from the provided {nameof(HttpContext)} instance.");


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
    /// Gets the Id of the current user if it is a customer.
    /// </summary>
    /// <param name="context">Context of the HTTP request which contains the user principal.</param>
    /// <returns>
    /// Task object representing the asynchronous operation. Value property gets a Result object
    /// that on success contains the Id of a user. If the user was not identified the Result object
    /// contains <see cref="UserNotIdentifiableError"/> error instance, or in case the user is not
    /// a customer, it contains <see cref="NotCustomerError"/>.
    /// </returns>
    Task<Result<string>> GetCustomerId(HttpContext context);

    /// <summary>
    /// Gets the name of the current user.
    /// </summary>
    /// <param name="context">Context of the HTTP request which contains the user principal.</param>
    /// <returns>Task object representing the asynchronous operation.</returns>
    Task<string?> GetUserNameAsync(HttpContext context);
}
