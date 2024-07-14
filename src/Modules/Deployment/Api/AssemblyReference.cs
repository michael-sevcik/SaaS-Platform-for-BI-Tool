using System.Reflection;

namespace BIManagement.Modules.Deployment.Api;

/// <summary>
/// Represents the Deployment API assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
