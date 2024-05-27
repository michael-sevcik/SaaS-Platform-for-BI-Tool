using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace BIManagement.Modules.Users.Api.PolicyAttributes;

/// <summary>
/// Custom authorization attribute that allows only admins to access the method.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AuthorizeOnlyCostumers : AuthorizeAttribute
{
    public AuthorizeOnlyCostumers() {
        base.Roles = Domain.Roles.Costumer;
    }
}

