using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
namespace BIManagement.Modules.Users.Api.PolicyAttributes;

/// <summary>
/// 
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
internal class AuthorizeOnlyCostumers : AuthorizeAttribute
{
    public AuthorizeOnlyCostumers()
    {
        // TODO:
        //Policy = Policy;
    }
}
