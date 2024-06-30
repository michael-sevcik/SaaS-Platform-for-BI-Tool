using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("SqlViewGeneratorTests")]

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing
{
    internal class SourceEntityReferenceHandler : ReferenceHandler
    {
        public SourceEntityReferenceHandler() => Reset();

        private ReferenceResolver? _rootedResolver;
        public override ReferenceResolver CreateResolver() => _rootedResolver!;
        public void Reset() => _rootedResolver = new MappingReferenceResolver();
    }
}
