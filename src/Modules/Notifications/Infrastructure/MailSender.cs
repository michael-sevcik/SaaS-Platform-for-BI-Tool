using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Exceptions;
using BIManagement.Modules.Notifications.Api;
using BIManagement.Modules.Notifications.Domain;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text.Encodings.Web;

namespace BIManagement.Modules.Notifications.Infrastructure;

/// <summary>
/// Default implementation of the <see cref="IEmailSender"/> interface using <see cref="SmtpClient"/>.
/// </summary>
sealed internal class EmailSender : IEmailSender, IScoped
{
    private readonly EmailOptions emailOptions;
    private readonly Uri baseUri;

    /// <summary>
    /// Initializes a new instance of the <see cref="EmailSender"/> class.
    /// </summary>
    /// <param name="options">The email options.</param>
    /// <exception cref="InvalidConfigurationException">Thrown when the provided base URL is not absolute.</exception>
    public EmailSender(IOptions<EmailOptions> options)
    {
        emailOptions = options.Value;

        baseUri = new Uri(emailOptions.BaseUrl);
        if (!baseUri.IsAbsoluteUri)
        {
            throw new InvalidConfigurationException("Provided base URL is not absolute.");
        }
    }

    /// <inheritdoc/>
    public async Task SendGeneralNotification(string email, string subject, string message)
    {
        MimeMessage mimeMessage = new();
        mimeMessage.Subject = subject;
        mimeMessage.To.Add(MailboxAddress.Parse(email));

        // Create body
        BodyBuilder bodyBuilder = new()
        {
            TextBody = message
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        await SendEmail(mimeMessage);
    }

    /// <inheritdoc/>
    public async Task SendInvitationLinkAsync(string email, string relativeLink)
    {
        Uri absoluteLink = new(baseUri, relativeLink);

        var encodedLink = HtmlEncoder.Default.Encode(absoluteLink.ToString());

        MimeMessage mimeMessage = new();
        mimeMessage.Subject = "Metabase SaaS platform invitation link";
        mimeMessage.To.Add(MailboxAddress.Parse(email));


        // Create body
        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = "Click the link below to accept the invitation to the Metabase SaaS platform: <br />" +
                $"<a href=\"{encodedLink}\">{encodedLink}</a> <br />" +
                "If you have any questions, please contact the support team. <br />" +
                "Best regards, <br />" +
                "The SaaS platform team."
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        await SendEmail(mimeMessage);
    }

    /// <inheritdoc/>
    public async Task SendPasswordResetCodeAsync(string email, string ResetCode)
    {
        MimeMessage mimeMessage = new();
        mimeMessage.Subject = "Metabase SaaS platform Reset Code"; ;
        mimeMessage.To.Add(MailboxAddress.Parse(email));

        // Create body
        BodyBuilder bodyBuilder = new()
        {
            TextBody = $"Your password reset code is: {ResetCode}"
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        await SendEmail(mimeMessage);
    }

    /// <inheritdoc/>
    public async Task SendPasswordResetLinkAsync(string email, string relativeResetLink)
    {
        Uri absoluteLink = new(baseUri, relativeResetLink);

        var encodedLink = HtmlEncoder.Default.Encode(absoluteLink.ToString());

        MimeMessage mimeMessage = new();
        mimeMessage.Subject = "Metabase SaaS platform invitation link";
        mimeMessage.To.Add(MailboxAddress.Parse(email));

        // Create body
        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = "Click the link below to reset your password to the Metabase SaaS platform: <br />" +
                $"<a href=\"{encodedLink}\">{encodedLink}</a> <br />" +
                "If you have any questions, please contact the support team. <br />" +
                "Best regards, <br />" +
                "The SaaS platform team."
        };

        mimeMessage.Body = bodyBuilder.ToMessageBody();
        await SendEmail(mimeMessage);
    }

    /// <inheritdoc/>
    public async Task SendEmail(MimeMessage message)
    {

        // Sender
        MailboxAddress sender = MailboxAddress.Parse(this.emailOptions.SenderEmail);
        message.From.Add(sender);
        message.Sender = sender;


        // Send the message
        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(emailOptions.SmtpServer, this.emailOptions.SmtpPort, SecureSocketOptions.None);

        if (emailOptions.SmtpUsername != string.Empty && emailOptions.SmtpPassword != string.Empty)
        {
            await smtp.AuthenticateAsync(this.emailOptions.SmtpUsername, this.emailOptions.SmtpPassword);
        }

        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
