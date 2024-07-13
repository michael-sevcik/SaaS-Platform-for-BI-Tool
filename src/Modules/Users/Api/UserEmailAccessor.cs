using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;

namespace BIManagement.Modules.Users.Api;

/// <summary>
/// Represents an interface for accessing user email addresses.
/// </summary>
/// <param name="userManager">The manager for users.</param>
internal class UserEmailAccessor(UserManager<ApplicationUser> userManager) : IUserEmailAccessor, IScoped
{
    public async Task<string?> GetUserEmailAsync(string userId)
    {
        var result = await userManager.FindByIdAsync(userId);
        return result?.Email;
    }
}
