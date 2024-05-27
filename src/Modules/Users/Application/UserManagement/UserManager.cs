using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Users.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Users.Application.UserManagement;

internal class UserManager : IUserManager, ISigleton
{
    public Task<Result<ApplicationUser>> CreateAdmin(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result<ApplicationUser>> CreateCostumerAsync(string email, string name)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        throw new NotImplementedException();
    }
}
