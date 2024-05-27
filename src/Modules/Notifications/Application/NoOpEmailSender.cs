using BIManagement.Modules.Notifications.Api;

namespace BIManagement.Modules.Notifications.Application;

// TODO: Change to full functional implementation
public sealed class NoOpEmailSender : IEmailSender
{
    public Task SendGeneralNotification(string email, string subject, string message)
    {
        return Task.CompletedTask;
    }

    public Task SendInvitationLinkAsync(string email, string link)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(string email, string resetCode)
    {
        return Task.CompletedTask;
    }

    public Task SendPasswrodResetLinkAsync(string email, string resetLink)
    {
        return Task.CompletedTask;
    }
}
