using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("SqlViewGeneratorTests")]

namespace BIManagement.Modules.DataIntegration.Application.Mapping.JsonParsing;

/// <summary>
/// Handles the reference to the source entity during JSON parsing.
/// </summary>
/// <remarks>Stores the references between deserialize calls.</remarks>
internal class MappingReferenceHandler : ReferenceHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingReferenceHandler"/> class.
    /// </summary>
    public MappingReferenceHandler() => Reset();

    private ReferenceResolver? _rootedResolver;

    /// <summary>
    /// Creates a new reference resolver.
    /// </summary>
    /// <returns>The created reference resolver.</returns>
    public override ReferenceResolver CreateResolver() => _rootedResolver!;

    /// <summary>
    /// Resets the reference resolver.
    /// </summary>
    public void Reset() => _rootedResolver = new MappingReferenceResolver();
}
