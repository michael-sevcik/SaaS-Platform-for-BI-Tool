using System.Reflection;

namespace BIManagement.Modules.Users.Infrastructure;

/// <summary>
/// Represents the Users infrastructure assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
