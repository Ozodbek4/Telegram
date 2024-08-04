using System.Linq.Expressions;
using Telegram.Domain.Entities;
using Telegram.Persistence.DataContexts;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Persistence.Repositories;

public class UserRepository : EntityRepositoryBase<User, TelegramDbContext>, IUserRepository
{
    public UserRepository(TelegramDbContext context) : base(context)
    {
    }

    public new IEnumerable<User> Get(Expression<Func<User, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.Get(predicate, asNoTracking, cancellationToken);

    public new ValueTask<IList<User>> GetAllAsync(bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetAllAsync(asNoTracking, cancellationToken);

    public new ValueTask<User?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetByIdAsync(id, asNoTracking, cancellationToken);

    public new ValueTask<IList<User>> GetByIdsAsync(IList<Guid> ids, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        base.GetByIdsAsync(ids, asNoTracking, cancellationToken);

    public new ValueTask<User> CreateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.CreateAsync(entity, saveChanges, cancellationToken);

    public new ValueTask<User?> DeleteByIdAsync(Guid entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.DeleteByIdAsync(entity, saveChanges, cancellationToken);

    public new ValueTask<User> UpdateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        base.UpdateAsync(entity, saveChanges, cancellationToken);
}