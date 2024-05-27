using BIManagement.Modules.Users.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BIManagement.Modules.Users.Persistence;

/// <summary>
/// 
/// </summary>
/// <param name="options"></param>
public class UsersContext(DbContextOptions<UsersContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(UsersSchema.Name);
        builder.Entity<IdentityRole>().HasData();   // todo: Add roles here
    }
}
