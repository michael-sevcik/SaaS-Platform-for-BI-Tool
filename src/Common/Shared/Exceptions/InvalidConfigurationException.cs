namespace BIManagement.Common.Shared.Exceptions;

/// <summary>
/// Represents errors in the configuration.
/// </summary>
public class InvalidConfigurationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
    /// </summary>
    public InvalidConfigurationException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The message describing the error which had occurred.</param>
    public InvalidConfigurationException(string message)
        : base(message)
    {
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidConfigurationException"/> class.
    /// </summary>
    /// <param name="message">The message describing the error which had occurred.</param>
    /// <param name="inner">The exception which is the cause of this exception.</param>
    public InvalidConfigurationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
