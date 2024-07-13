using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BIManagement.Modules.Users.Infrastructure.Identity
{
    /// <summary>
    /// Email sender
    /// </summary>
    /// <param name="emailSender"></param>
    internal sealed class IdentityEmailSender(Notifications.Api.IEmailSender emailSender) : IEmailSender<ApplicationUser>
    {
        public Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink) =>
            emailSender.SendInvitationLinkAsync(email, confirmationLink);

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) =>
            emailSender.SendPasswordResetLinkAsync(email, resetLink);

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) =>
            emailSender.SendPasswordResetCodeAsync(email, resetCode);
    }
}
