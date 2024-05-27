namespace BIManagement.Modules.Notifications.Api
{
    public interface IEmailSender
    {
        /// <summary>
        /// Sends an invitation link to the user.
        /// </summary>
        /// <remarks>
        /// Intended to be used for inviting costumers.
        /// </remarks>
        /// <param name="email"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        Task SendInvitationLinkAsync(string email, string link);

        /// <summary>
        /// Sends a password reset link to the user.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="resetLink">The link to follow to reset the user password. Do not double encode this.</param>
        /// <returns>Task object representing the asynchronous operation.</returns>
        Task SendPasswrodResetLinkAsync(string email, string resetLink);

        /// <summary>
        /// Sends a password reset code to the user.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="resetCode">The code to use to reset the user password. Do not double encode this.</param>
        /// <returns>Task object representing the asynchronous operation.</returns>
        Task SendPasswordResetCodeAsync(string email, string resetCode);

        /// <summary>
        /// Sends a password reset code to the user.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject of the email notification.</param>
        /// <param name="message">The message of the email</param>
        /// <returns>Task object representing the asynchronous operation.</returns>
        Task SendGeneralNotification(string email, string subject, string message);
    }
}
