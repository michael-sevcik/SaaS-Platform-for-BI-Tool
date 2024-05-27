using BIManagement.Common.Shared.Results;

namespace BIManagement.Modules.Users.Domain;

/// <summary>
/// Represents the user repository interface.
/// </summary>
public interface IUserRepository
{
    Task<Result<ApplicationUser>> GetAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<ApplicationUser>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    //Task<Result<ApplicationUser>> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);
    Task<Result> AddAsync(ApplicationUser user, Role role, CancellationToken cancellationToken = default);

    //Task<Result> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<Result> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<ApplicationUser>>> GetUsersByRole(Role role, CancellationToken cancellationToken = default);
}
