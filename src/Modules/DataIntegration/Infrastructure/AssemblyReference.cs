using System.Reflection;

namespace BIManagement.Modules.DataIntegration.Infrastructure;

/// <summary>
/// Represents the Data integration infrastructure assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
