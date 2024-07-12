using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace BIManagement.Modules.Users.Api.Authorization.PolicyAttributes;

/// <summary>
/// Custom authorization attribute that allows only customers to access the method.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AuthorizeOnlyCustomersAttribute : AuthorizeAttribute
{
    public AuthorizeOnlyCustomersAttribute()
    {
        Roles = Domain.Roles.Customer;
    }
}

