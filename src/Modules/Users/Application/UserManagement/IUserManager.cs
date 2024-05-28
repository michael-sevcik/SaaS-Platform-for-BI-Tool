using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Users.Domain;

namespace BIManagement.Modules.Users.Application.UserManagement;

/// <summary>
/// Represents the user manager interface.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Creates a new costumer and sends him an invite notification.
    /// </summary>
    /// <param name="email">Costumer's email.</param>
    /// <param name="name">Name of the costumer</param>
    /// <returns>
    /// Task object representing the asynchronous operation
    /// with <see cref="Result{ApplicationUser}"/> as its value.
    /// Result object either contains the created user or an error.
    /// </returns>
    Task<Result<ApplicationUser>> CreateCostumerAsync(string email, string name);

    /// <summary>
    /// Creates a new user of type admin asynchronously.
    /// </summary>
    /// <param name="email">The admin's email.</param>
    /// <param name="password">The password of the admin</param>
    /// <returns>
    /// Task object representing the asynchronous operation
    /// with <see cref="Result{ApplicationUser}"/> as its value.
    /// Result object either contains the created user or an error.
    /// </returns>
    Task<Result<ApplicationUser>> CreateAdmin(string email, string password);

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
    Task<Result<ApplicationUser>> GetUser(string Id);
}
