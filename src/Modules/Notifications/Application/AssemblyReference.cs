using System.Reflection;

namespace BIManagement.Modules.Notifications.Application;

/// <summary>
/// Represents the Notifications application assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
