using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Users.Api.PolicyAttributes;

/// <summary>
/// Custom authorization attribute that allows only admins to access the method.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AuthorizeOnlyAdmins : AuthorizeAttribute
{
    public AuthorizeOnlyAdmins()
    {
        base.Roles = Domain.Roles.Admin;
    }
}
