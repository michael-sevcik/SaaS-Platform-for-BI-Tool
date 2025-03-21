﻿using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BIManagement.Modules.Users.Application.UserManagement;

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
    IIntegrationNotifier integrationNotifier,
    ILogger<UserManager> logger
    ) : IUserManager, IScoped
{
    public async Task<Result<ApplicationUser>> GetUser(string Id)
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
    public async Task<Result<ApplicationUser>> GetCustomer(string Id)
        => await GetUser(Id).Bind(async user => await CheckThatUserIsInRole(user, Roles.Customer));

    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> GetAdmin(string Id)
        => await GetUser(Id).Bind(async user => await CheckThatUserIsInRole(user, Roles.Admin));


    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> CreateAdminAsync(string email, string name)
        => await CreateUser(email, name, Roles.Admin);

    /// <inheritdoc/>
    public async Task<Result<ApplicationUser>> CreateCustomerAsync(string email, string name)
        => await CreateUser(email, name, Roles.Customer);

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

        // create a new user
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

        // register the user in the given role
        await userManager.AddToRoleAsync(user, role);
        logger.LogInformation("Created a new user account with a given role.");

        // send an invitation email
        var userId = await userManager.GetUserIdAsync(user);
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var relativeCallbackUrl = QueryHelpers.AddQueryString("/Account/ConfirmEmailAndSetPassword", "userId", userId);
        relativeCallbackUrl = QueryHelpers.AddQueryString(relativeCallbackUrl, "code", code);

        await emailSender.SendInvitationLinkAsync(email, relativeCallbackUrl);

        return Result.Success(user);
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(string id)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
        {
            return Result.Failure(UserErrors.UserNotFoundById);
        }

        return await DeleteUserAsync(user);
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        bool sendNotification = false;
        if (await userManager.IsInRoleAsync(user, Roles.Customer))
        {
            sendNotification = true;
        }

        var result = await userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            logger.LogError("Error deleting user: {Errors}", result.Errors);
            return Result.Failure(UserErrors.UserDeletionFailed);
        }

        if (sendNotification)
        {
            await integrationNotifier.SentCustomerDeletionNotification(user.Id);
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
