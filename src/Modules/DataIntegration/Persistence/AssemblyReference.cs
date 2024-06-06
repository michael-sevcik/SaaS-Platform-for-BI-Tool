using System.Reflection;

namespace BIManagement.Modules.DataIntegration.Persistence;

/// <summary>
/// Represents the Data integration persistence assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
