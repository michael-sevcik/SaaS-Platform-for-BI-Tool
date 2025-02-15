﻿using System.Reflection;

namespace BIManagement.Modules.Notifications.Infrastructure;

/// <summary>
/// Represents the Notifications infrastructure assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
