using System.Reflection;

namespace BIManagement.Modules.DataIntegration.Application;

/// <summary>
/// Represents the Data integration Application layer assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
