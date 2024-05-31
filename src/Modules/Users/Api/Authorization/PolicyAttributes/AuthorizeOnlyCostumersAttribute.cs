using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace BIManagement.Modules.Users.Api.Authorization.PolicyAttributes;

/// <summary>
/// Custom authorization attribute that allows only admins to access the method.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeOnlyCostumersAttribute : AuthorizeAttribute
{
    public AuthorizeOnlyCostumersAttribute()
    {
        Roles = Domain.Roles.Costumer;
    }
}

