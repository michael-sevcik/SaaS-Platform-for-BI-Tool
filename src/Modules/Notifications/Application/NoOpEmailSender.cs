using BIManagement.Common.Shared.Exceptions;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Notifications.Domain;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace BIManagement.Modules.Notifications.Application;


/// <summary>
/// Development only email sender that stores the messages.
/// </summary>
public sealed class NoOpEmailSender : IEmailSender
{
    private readonly EmailOptions emailOptions;
    private readonly Uri baseUri;
    private static readonly List<string> Messages = [];

    public NoOpEmailSender(IOptions<EmailOptions> emailConfiguration)
    {
        emailOptions = emailConfiguration.Value;
        baseUri = new Uri(emailConfiguration.Value.BaseUrl);
        if (!baseUri.IsAbsoluteUri)
        {
            throw new InvalidConfigurationException("Provided base URL is not absolute.");
        }
    }

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

    public Task SendInvitationLinkAsync(string email, string relativeLink)
    {

        Uri absoluteLink = new(baseUri, relativeLink);

        // TODO: USE for final implementation
        var encodedLink = HtmlEncoder.Default.Encode(absoluteLink.ToString());

        Messages.Add($"""
            Email: {email}
            Message: {absoluteLink}
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
