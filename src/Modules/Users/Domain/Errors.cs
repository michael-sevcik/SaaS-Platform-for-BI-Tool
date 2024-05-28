using BIManagement.Common.Shared.Errors;

namespace BIManagement.Modules.Users.Domain;

public class UserErrors
{
    public static readonly Error UserCreationFailed = new("Error.UserCreationFailed", Resources.UserErrors.UserCreationFailed);
    public static readonly Error UserNotFoundById = new("Error.UserNotFoundById", Resources.UserErrors.UserNotFoundById);
}
