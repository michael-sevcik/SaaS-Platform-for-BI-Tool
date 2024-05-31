using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace BIManagement.Modules.Users.Application.UserManagement;

// TODO: put the error messages in the resources.
/// <summary>
/// 
/// </summary>
/// <param name="userManager"></param>
/// <param name="userStore"></param>
/// <param name="emailSender"></param>
/// <param name="logger"></param>
internal sealed class UserManager(
    UserManager<ApplicationUser> userManager,
    IUserStore<ApplicationUser> userStore,
    IEmailSender emailSender,
    IConfiguration configuration,
    IIntegrationNotifier integrationNotifier,
    ILogger<UserManager> logger
    ) : IUserManager, IScoped
{
    private async Task<Result<ApplicationUser>> GetUser(string Id)
       => await userManager.FindByIdAsync(Id) switch
       {
           null => Result.Failure<ApplicationUser>(UserErrors.UserNotFoundById),
           var user => Result.Success(user)
       };

    private async Task<Result<ApplicationUser>> CheckThatUserIsInRole(ApplicationUser user, Role role)
        => await userManager.IsInRoleAsync(user, role) 
            ? Result.Success(user) 
            : Result.Failure<ApplicationUser>(UserErrors.UserNotFoundById);


    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> GetCostumer(string Id)
        => await GetUser(Id).Bind(async user => await CheckThatUserIsInRole(user, Roles.Costumer));

    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> GetAdmin(string Id)
        => await GetUser(Id).Bind(async user => await CheckThatUserIsInRole(user, Roles.Admin));


    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> CreateAdminAsync(string email, string name)
        => await CreateUser(email, name, Roles.Admin);

    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> CreateCostumerAsync(string email, string name)
        => await CreateUser(email, name, Roles.Costumer);

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
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        var emailStore = GetEmailStore();

        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        user.Name = name;
        await userStore.UpdateAsync(user, CancellationToken.None);

        var result = await userManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("Error creating user: {Errors}", result.Errors);
            return Result.Failure<ApplicationUser>(UserErrors.UserCreationFailed);
        }

        await userManager.AddToRoleAsync(user, role);

        logger.LogInformation("Created a new user account with a given role.");

        var userId = await userManager.GetUserIdAsync(user);
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        if (!Uri.TryCreate(configuration["ManagementApp:FrontendUrl"], UriKind.Absolute, out var frontendUrl) || frontendUrl is null)
        {
            throw new InvalidOperationException("The ManagementApp:FrontendUrl is not specified in configuration or it has invalid format.");
        }

        var uriBuilder = new UriBuilder(frontendUrl) { Path = "/Account/ConfirmEmail" };
        var callbackUrl = uriBuilder.Uri.ToString();
        callbackUrl = QueryHelpers.AddQueryString(callbackUrl, "userId", userId);
        callbackUrl = QueryHelpers.AddQueryString(callbackUrl, "code", code);

        await emailSender.SendInvitationLinkAsync(email, callbackUrl);

        return Result.Success(user);
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return Result.Failure(new("Error.UserNotFound", "Given user was not found."));
        }

        return await DeleteUserAsync(user);
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("Error deleting user: {Errors}", result.Errors);
            return Result.Failure(UserErrors.UserDeletionFailed);
        }

        if (await userManager.IsInRoleAsync(user, Roles.Costumer))
        {
            await integrationNotifier.SentCostumerDeletionNotification(user.Id);
        }

        return Result.Success();
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
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)userStore;
    }

    /// <inheritdoc/>
    public async Task<IList<ApplicationUser>> GetUsersByRoleAsync(Role role)
        => await userManager.GetUsersInRoleAsync(role);

    /// <inheritdoc/>
    public async Task<ApplicationUser?> GetUserByEmail(string email)
        => await userManager.FindByEmailAsync(email);

}
