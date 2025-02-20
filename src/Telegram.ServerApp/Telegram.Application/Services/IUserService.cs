using Telegram.Application.Common.Models;
using Telegram.Domain.Entities;

namespace Telegram.Application.Services;

public interface IUserService
{
    Task<PaginationResult<User>> GetAllAsync(
        PaginationParameters pagination,
        SortingParameters sorting,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<User> GetByIdAsync(long id, bool asNoTracking = true, CancellationToken cancellationToken = default);

    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}