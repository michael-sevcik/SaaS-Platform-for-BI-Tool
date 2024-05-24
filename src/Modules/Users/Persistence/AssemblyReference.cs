﻿using System.Reflection;

namespace BIManagement.Modules.Users.Persistence;

/// <summary>
/// Represents the Users persistence assembly reference.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// The assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
