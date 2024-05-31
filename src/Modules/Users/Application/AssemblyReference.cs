using System.Reflection;

namespace BIManagement.Modules.Users.Application;

/// <summary>
/// Represents the Users application layer assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
