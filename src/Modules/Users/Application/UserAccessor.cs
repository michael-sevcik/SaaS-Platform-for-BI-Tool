using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Users.Api;
using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BIManagement.Modules.Users.Application;

/// <summary>
/// Represents helper class that provides access to the current user.
/// </summary>
internal class UserAccessor(UserManager<ApplicationUser> userManager) : IUserAccessor, IScoped
{
    public async Task<Result<string>> GetCostumerId(HttpContext context)
    {
        var user = await GetUser(context);

        if (user is null)
        {
            return Result.Failure<string>(IUserAccessor.UserNotIdentifiableError);
        }

        if (!await userManager.IsInRoleAsync(user, Roles.Costumer))
        {
            return Result.Failure<string>(IUserAccessor.NotCustomerError);
        }

        return Result.Success(user.Id);
    }

    /// <inheritdoc/>
    public async Task<string?> GetUserEmailAsync(HttpContext context)
        => (await GetUser(context))?.Email;


    /// <inheritdoc/>
    public async Task<string?> GetUserIdAsync(HttpContext context)
        => (await GetUser(context))?.Id;


    /// <inheritdoc/>
    public async Task<string?> GetUserNameAsync(HttpContext context)
        => (await GetUser(context))?.Name;

    private async Task<ApplicationUser?> GetUser(HttpContext context)
        => await userManager.GetUserAsync(context.User);
}
