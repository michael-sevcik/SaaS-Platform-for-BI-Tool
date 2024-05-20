using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator;

public interface IVisitable
{
    void Accept(IVisitor visitor);
}
