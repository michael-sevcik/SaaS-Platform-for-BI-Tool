using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BIManagement.Modules.Users.Application.UserManagement;

internal class UserManager(
    UserManager<ApplicationUser> UserManager,
    IUserStore<ApplicationUser> UserStore,
    IEmailSender EmailSender,
    NavigationManager NavigationManager,
    ILogger<UserManager> Logger
    ) : IUserManager, IScoped
{
    public Task<Result<ApplicationUser>> CreateAdmin(string email, string password)
    {
        throw new NotImplementedException();

    }

    public async Task<Result<ApplicationUser>> CreateCostumerAsync(string email, string name)
    {
        ApplicationUser user = new();

        await UserStore.SetUserNameAsync(user, email, CancellationToken.None);
        var emailStore = GetEmailStore();
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);

        var result = await UserManager.CreateAsync(user);

        if (!result.Succeeded)
        {
            // CONSIDER : Externalize the error messages.
            return Result.Failure<ApplicationUser>(new("Error.UserCreationFailed", result.ToString()));
        }

        Logger.LogInformation("User created a new account with password.");

        var userId = await UserManager.GetUserIdAsync(user);
        var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = NavigationManager.GetUriWithQueryParameters(
            NavigationManager.ToAbsoluteUri("Account/ConfirmEmail").AbsoluteUri,
            new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code });

        await EmailSender.SendInvitationLinkAsync(email, HtmlEncoder.Default.Encode(callbackUrl));

        return Result.Success(user);
    }

    public Task<Result> DeleteUserAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        throw new NotImplementedException();
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!UserManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)UserStore;
    }
}
