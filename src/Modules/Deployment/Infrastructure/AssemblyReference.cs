using System.Reflection;

namespace BIManagement.Modules.Deployment.Infrastructure;

/// <summary>
/// Represents the Deployment infrastructure assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
