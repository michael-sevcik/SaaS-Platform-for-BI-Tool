using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIManagement.Modules.DataIntegration.Domain.JsonModel.SourceEntities;

namespace BIManagement.Modules.DataIntegration.Domain.JsonModel.SourceEntities.Agregators;

public interface IAgregator : ISourceEntity
{
    ISourceEntity[] SourceEntities { get; }

}
