using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Domain.Roles;
namespace Api.PolicyAttributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal class AuthorizeOnlyCostumers : AuthorizeAttribute
{
    public AuthorizeOnlyCostumers()
    {
        this.Policy = Policy.;
    }
}
