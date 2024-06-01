namespace BIManagement.Modules.Users.Domain;

/// <summary>
/// Represents a user role.
/// </summary>
/// <param name="name"></param>
public readonly struct Role(string name)
{
    public readonly string Name = name;
    public static implicit operator string(Role role) => role.Name;
}

/// <summary>
/// Represents the types of user roles.
/// </summary>
public static class Roles
{
    /// <summary>
    /// Represents the admin role.
    /// </summary>
    public static readonly Role Admin = new ("Admin");

    /// <summary>
    /// Represents the costumer role.
    /// </summary>
    public static readonly Role Costumer = new ("Costumer");
}
