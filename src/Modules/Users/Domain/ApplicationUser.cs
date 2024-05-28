using BIManagement.Common.Domain.Primitives;
using Microsoft.AspNetCore.Identity;

namespace BIManagement.Modules.Users.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser, IAuditable
    {
        /// <summary>
        /// Represents the maximum length of the <see cref="Name"/> property.
        /// </summary>
        public const int NameMaxLength = 70;

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <remarks>
        /// Max length of <see cref="NameMaxLength"/>
        /// </remarks>
        public string Name { get; set; } = string.Empty;

        // TODO: fINISH IMPLEMENTATION OF IAuditable
        DateTime IAuditable.CreatedOnUtc => throw new NotImplementedException();

        DateTime? IAuditable.ModifiedOnUtc => throw new NotImplementedException();
    }

}
