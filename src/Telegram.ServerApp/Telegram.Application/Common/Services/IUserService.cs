using System.Linq.Expressions;
using Telegram.Domain.Entities;

namespace Telegram.Application.Common.Services;

public interface IUserService
{
    IEnumerable<User> Get(Expression<Func<User, bool>>? predicate = default, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<User?> GetByUserNameAsync(string userName, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<User?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default);

    ValueTask<User> CreateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<User> UpdateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    ValueTask<User> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default);
}