using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("SqlViewGeneratorTests")]

namespace SqlViewGenerator.MappingParser
{
    internal class SourceEntityReferenceHandler : ReferenceHandler
    {
        public SourceEntityReferenceHandler() => Reset();

        private ReferenceResolver? _rootedResolver;
        public override ReferenceResolver CreateResolver() => _rootedResolver!;
        public void Reset() => _rootedResolver = new SourceEntityReferenceResolver();
    }
}
