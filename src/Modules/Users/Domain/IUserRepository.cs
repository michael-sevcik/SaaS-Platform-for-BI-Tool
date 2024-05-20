using BIManagement.Common.Shared.Results;
using BIManagementPlatform.Modules.Users.Domain.Roles;

namespace BIManagementPlatform.Modules.Users.Domain;

/// <summary>
/// Represents the user repository interface.
/// </summary>
public interface IUserRepository
{
    Task<Result<ApplicationUser>> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUser>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<ApplicationUser>>> GetUsersByRole(Role role, CancellationToken cancellationToken = default);
}
