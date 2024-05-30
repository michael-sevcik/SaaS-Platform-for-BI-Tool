using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Reflection.Emit;

namespace BIManagement.Modules.Users.Persistence;

/// <summary>
/// Database context for the users module. It is derived from the <see cref="IdentityDbContext{ApplicationUser}"/> class."/>
/// </summary>
/// <param name="options">Options for the EF core.</param>
public class UsersContext(DbContextOptions<UsersContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(UsersSchema.Name);
        // TODO: CREATE A SEEDING SERVICE to create an admin user
        builder.Entity<ApplicationUser>().Property(p => p.Name).IsRequired().HasMaxLength(ApplicationUser.NameMaxLength);

        //builder.Entity<IdentityRole>().HasData(new IdentityRole(Domain.Roles.Admin), new IdentityRole(Domain.Roles.Costumer));
    }
}
