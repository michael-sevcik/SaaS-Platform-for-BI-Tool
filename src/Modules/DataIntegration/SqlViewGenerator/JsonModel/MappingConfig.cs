using BIManagement.Modules.DataIntegration.SqlViewGenerator;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.SqlViewGenerator.JsonModel;

public class MappingConfig : IVisitable
{
    public MappingConfig(DbConnectionConfig sourceConnection, EntityMapping[] targetMappings)
        => (SourceConnection, TargetMappings) = (sourceConnection, targetMappings);

    public DbConnectionConfig SourceConnection { get; }

    public EntityMapping[] TargetMappings { get; }

    public void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
