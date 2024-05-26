namespace BIManagement.Common.Shared.Errors;

/// <summary>
/// Represents the conflict error.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConflictError"/> class.
/// </remarks>
/// <param name="code">The error code.</param>
/// <param name="message">The error message.</param>
public sealed class ConflictError(string code, string message) : Error(code, message);
