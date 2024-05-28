using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BIManagement.Modules.Users.Application.UserManagement;

// TODO: put the error messages in the resources.
/// <summary>
/// 
/// </summary>
/// <param name="UserManager"></param>
/// <param name="UserStore"></param>
/// <param name="EmailSender"></param>
/// <param name="NavigationManager"></param>
/// <param name="Logger"></param>
internal class UserManager(
    UserManager<ApplicationUser> UserManager,
    IUserStore<ApplicationUser> UserStore,
    IEmailSender EmailSender,
    NavigationManager NavigationManager,
    ILogger<UserManager> Logger
    ) : IUserManager, IScoped
{
    public async Task<Result<ApplicationUser>> GetUser(string Id)
        => await UserManager.FindByIdAsync(Id) switch 
        {
            null => Result.Failure<ApplicationUser>(UserErrors.UserNotFoundById),
            var user => Result.Success(user)
        };

    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> CreateAdmin(string email, string name)
        => await CreateUser(email, name, Roles.Admin);

    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> CreateCostumerAsync(string email, string name)
        => await CreateUser(email, email, Roles.Costumer);

    /// <summary>
    /// Creates an user with the given email, name and role.
    /// </summary>
    /// <param name="email">The user's email.</param>
    /// <param name="name">The user's name.</param>
    /// <param name="role">The role of the user.</param>
    /// <returns>
    /// Task object representing the asynchronous operation
    /// with <see cref="Result{ApplicationUser}"/> as its value.
    /// Result object either contains the created user or an error.
    /// </returns>
    private async Task<Result<ApplicationUser>> CreateUser(string email, string name, Role role)
    {
        ApplicationUser user = new();

        // TODO: consider using the cancellation tokens
        await UserStore.SetUserNameAsync(user, email, CancellationToken.None);
        var emailStore = GetEmailStore();

        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        user.Name = name;
        await UserStore.UpdateAsync(user, CancellationToken.None);

        var result = await UserManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            Logger.LogError("Error creating user: {0}", result.Errors);
            return Result.Failure<ApplicationUser>(UserErrors.UserCreationFailed);
        }

        await UserManager.AddToRoleAsync(user, role);

        Logger.LogInformation("Created a new user account with a given role.");

        var userId = await UserManager.GetUserIdAsync(user);
        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri, // TODO: CONSIDER moving this to a constant.
            new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code });

        await EmailSender.SendInvitationLinkAsync(email, HtmlEncoder.Default.Encode(callbackUrl));

        return Result.Success(user);
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(string id)
    {
        var user = await UserManager.FindByIdAsync(id);
        if (user is null)
        {
            return Result.Failure(new("Error.UserNotFound", "Given user was not found."));
        }

        return await DeleteUserAsync(user);
    }

    /// <inheritdoc/>
    public Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        // TODO: IMPLEMENT THIS METHDO.
        throw new NotImplementedException();
    }

    /// <summary>
    /// Helper method to get the email store.
    /// </summary>
    /// <returns>An instance of <see cref="IUserEmailStore{ApplicationUser}"/>.</returns>
    /// <exception cref="NotSupportedException">
    /// The instance of the injected <see cref="IUserEmailStore{ApplicationUser}"/>
    /// does not support emails.
    /// </exception>
    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!UserManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)UserStore;
    }

    /// <inheritdoc/>
    public async Task<IList<ApplicationUser>> GetUsersByRoleAsync(Role role)
        => await UserManager.GetUsersInRoleAsync(role);
}
