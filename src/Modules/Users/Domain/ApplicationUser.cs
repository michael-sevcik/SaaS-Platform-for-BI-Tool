using BIManagement.Common.Domain.Primitives;
using BIManagementPlatform.Modules.Users.Domain.Roles;
using Microsoft.AspNetCore.Identity;

namespace BIManagementPlatform.Modules.Users.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IAuditable
    {
        public Role Role { get; set; }

        // TODO: fINISH IMPLEMENTATION OF IAuditable
        DateTime IAuditable.CreatedOnUtc =>  throw new NotImplementedException();

        DateTime? IAuditable.ModifiedOnUtc => throw new NotImplementedException();
    }

}
