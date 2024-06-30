using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping.JsonModel.SourceEntities
{
    public class CustomQuery : ISourceEntity
    {
        public const string TypeDiscriminator = "customQuery";

        public string Name => throw new NotImplementedException();

        public bool HasDependency => throw new NotImplementedException();

        public string[] SelectedColumns => throw new NotImplementedException();

        public void Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }
    }
}
