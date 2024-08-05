using System.Linq.Expressions;
using Telegram.Application.Common.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Infrastructure.Common.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public IEnumerable<User> Get(Expression<Func<User, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        userRepository.Get(predicate, asNoTracking, cancellationToken);

    public ValueTask<IList<User>> GetAllAsync(bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        userRepository.GetAllAsync(asNoTracking, cancellationToken);

    public ValueTask<User?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        userRepository.GetByIdAsync(id, asNoTracking, cancellationToken);

    public ValueTask<IList<User>> GetByIdsAsync(IList<Guid> ids, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        userRepository.GetByIdsAsync(ids, asNoTracking, cancellationToken);

    public ValueTask<User> CreateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        userRepository.CreateAsync(entity, saveChanges, cancellationToken);

    public ValueTask<User> UpdateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        userRepository.UpdateAsync(entity, saveChanges, cancellationToken);

    public ValueTask<User?> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        userRepository.DeleteByIdAsync(entity, saveChanges, cancellationToken);
}