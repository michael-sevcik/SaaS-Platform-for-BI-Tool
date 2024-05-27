using BIManagement.Common.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace BIManagement.Modules.Users.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IAuditable
    {
        // TODO: fINISH IMPLEMENTATION OF IAuditable
        DateTime IAuditable.CreatedOnUtc => throw new NotImplementedException();

        DateTime? IAuditable.ModifiedOnUtc => throw new NotImplementedException();
    }

}
