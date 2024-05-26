using BIManagement.Common.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BIManagement.Modules.Users.Domain.Roles
{
    public class Policy(int id, string name) : Enumeration<Policy>(id, name)
    {
        public readonly static Policy AdminOnly = new((int)Role.Admin, "Admin");
        public readonly static Policy costumerOnly = new((int)Role.Costumer, "Costumer");
    }
}
