using Telegram.Domain.Entities;
using Telegram.Persistence.Repositories;

namespace Telegram.Persistence.UnitOfWorks;

public interface IUnitOfWork : IDisposable
{
    // repositories
    IRepository<User> Users { get; }

    IRepository<ChatRoom> ChatRooms { get; }
    
    IRepository<Message> Messages { get; }

    // methods
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}