using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagementPlatform.Modules.DataIntegration.SqlViewGenerator.JsonModel;

public interface IAgregator : ISourceEntity
{
    ISourceEntity[] SourceEntities { get; }

}
