using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Application.Time;

namespace BIManagement.Common.Infrastructure.Time;

/// <summary>
/// Represents the system time interface.
/// </summary>
public sealed class SystemTime : ISystemTime, ITransient
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}
