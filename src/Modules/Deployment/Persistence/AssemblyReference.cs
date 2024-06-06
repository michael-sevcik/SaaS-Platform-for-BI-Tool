using System.Reflection;

namespace BIManagement.Modules.Deployment.Persistence;

/// <summary>
/// Represents the Deployment persistence assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
