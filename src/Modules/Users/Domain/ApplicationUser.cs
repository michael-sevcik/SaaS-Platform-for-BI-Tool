using Microsoft.AspNetCore.Identity;

namespace BIManagement.Modules.Users.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
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
        [ProtectedPersonalData]
        public string Name { get; set; } = string.Empty;
    }

}
