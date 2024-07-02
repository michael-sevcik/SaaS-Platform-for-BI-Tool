using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Users.Domain;

namespace BIManagement.Modules.Users.Application.UserManagement;

/// <summary>
/// Represents the user manager interface.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Creates a new customer and sends him an invite notification.
    /// </summary>
    /// <param name="email">Customer's email.</param>
    /// <param name="name">Name of the customer</param>
    /// <returns>
    /// Task object representing the asynchronous operation
    /// with <see cref="Result{ApplicationUser}"/> as its value.
    /// Result object either contains the created user or an error.
    /// </returns>
    Task<Result<ApplicationUser>> CreateCustomerAsync(string email, string name);

    /// <summary>
    /// Creates a new user of type admin asynchronously.
    /// </summary>
    /// <param name="email">The admin's email.</param>
    /// <param name="name">Name of the admin</param>
    /// <returns>
    /// Task object representing the asynchronous operation
    /// with <see cref="Result{ApplicationUser}"/> as its value.
    /// Result object contains either the created user or an error.
    /// </returns>
    Task<Result<ApplicationUser>> CreateAdminAsync(string email, string name);

    /// <summary>
    /// Removes a user from the system.
    /// </summary>
    /// <remarks>
    /// Removing the user from the system means that the data associated
    /// with the user in both this module and other modules will be deleted.
    /// </remarks>
    /// <param name="id">The Id of the user </param>
    /// <returns>
    /// Task object representing the asynchronous operation
    /// with <see cref="Result"/> as its value.
    /// Result is either a success or an error.
    /// </returns>
    Task<Result> DeleteUserAsync(string id);


    /// <summary>
    /// Removes a user from the system.
    /// </summary>
    /// <param name="user">The user to delete.</param>
    /// <returns>
    /// Task object representing the asynchronous operation
    /// with <see cref="Result"/> as its value.
    /// Result is either a success or an error.
    /// </returns>
    Task<Result> DeleteUserAsync(ApplicationUser user);

    /// <summary>
    /// Asynchronously gets all users by role.
    /// </summary>
    /// <param name="role">The role of the users to get.</param>
    /// <returns>    
    /// Task object representing the asynchronous operation
    /// with <see cref="IList{ApplicationUser}"/> as its value.
    /// </returns>
    Task<IList<ApplicationUser>> GetUsersByRoleAsync(Role role);

    /// <summary>Asynchronously gets an user of role Admin with the specified email.</summary>
    /// <param name="id">The id of the desired user.</param>
    /// <returns>    
    /// Task object representing the asynchronous operation
    /// with <see cref="Result{ApplicationUser}"/> as its value.
    /// The result object contains either the user or an error.
    /// </returns>
    Task<Result<ApplicationUser>> GetAdmin(string id);

    /// <summary>Asynchronously gets an user of role Customer with the specified email.</summary>
    /// <param name="id">The id of the desired user.</param>
    /// <returns>    
    /// Task object representing the asynchronous operation
    /// with <see cref="Result{ApplicationUser}"/> as its value.
    /// The result object contains either the user or an error.
    /// </returns>
    Task<Result<ApplicationUser>> GetCustomer(string id);

    /// <summary>Asynchronously gets user with the specified email.</summary>
    /// <param name="email">The email address of the desired user.</param>
    /// <returns>    
    /// Task object representing the asynchronous operation
    /// with <see cref="ApplicationUser"/> as its value if the user was found,
    /// otherwise null..
    /// </returns>
    Task<ApplicationUser?> GetUserByEmail(string email);
}
