using BIManagement.Common.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BIManagement.Modules.Users.Domain.Roles
{
    public class Policy : Enumeration<Policy>
    {
        public static Policy AdminOnly = new Policy((int)Role.Admin, "Admin");
        public static Policy CostumerOnly = new Policy((int)Role.Costumer, "Costumer");

        public Policy(int id, string name) : base(id, name)
        {
        }
    }
}
