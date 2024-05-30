using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Users.Domain;

public interface IIntegrationNotifier
{
    /// <summary>
    /// Notifies other modules about the user deletion.
    /// </summary>
    /// <param name="userId">Id of the deleted user.</param>
    /// <returns>Task object representing the asynchronous operation.</returns>
    Task SentUserDeletionNotification(string userId);
}
