using System.Reflection;

namespace BIManagement.Modules.Deployment.Application;

/// <summary>
/// Represents the Deployment Application assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
