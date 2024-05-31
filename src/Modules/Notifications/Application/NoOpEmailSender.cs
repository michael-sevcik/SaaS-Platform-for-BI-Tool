using BIManagement.Modules.Notifications.Api;

namespace BIManagement.Modules.Notifications.Application;


/// <summary>
/// Development only email sender that stores the messages.
/// </summary>
public sealed class NoOpEmailSender : IEmailSender
{
    private static readonly List<string> Messages = [];

    public static IReadOnlyList<string> GetMessages() => Messages;
    public Task SendGeneralNotification(string email, string subject, string message)
    {
        Messages.Add($"""
            Email: {email}
            Subject: {subject}
            Message: {message}
            """);

        Console.WriteLine(Messages.LastOrDefault());

        return Task.CompletedTask;
    }

    public Task SendInvitationLinkAsync(string email, string link)
    {
        Messages.Add($"""
            Email: {email}
            Message: {link}
            """);

        Console.WriteLine(Messages.LastOrDefault());

        return Task.CompletedTask;
    }

    public Task SendPasswordResetCodeAsync(string email, string resetCode)
    {
        Messages.Add($"""
            Email: {email}
            reset code: {resetCode}
            """);

        Console.WriteLine(Messages.LastOrDefault());

        return Task.CompletedTask;
    }

    public Task SendPasswrodResetLinkAsync(string email, string resetLink)
    {
        Messages.Add($"""
            Email: {email}
            Message: {resetLink}
            """);

        Console.WriteLine(Messages.LastOrDefault());

        return Task.CompletedTask;
    }
}
