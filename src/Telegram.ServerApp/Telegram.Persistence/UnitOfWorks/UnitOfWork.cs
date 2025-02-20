using Telegram.Domain.Entities;
using Telegram.Persistence.DataContexts;
using Telegram.Persistence.Repositories;

namespace Telegram.Persistence.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext appDbContext, IRepository<User> users)
    {
        _context = appDbContext;
        Users = users;
    }

    // repositories
    public IRepository<User> Users { get; }

    // methods
    public void Dispose() =>
        GC.SuppressFinalize(this);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _context.SaveChangesAsync(cancellationToken);

    public Task BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        _context.Database.BeginTransactionAsync(cancellationToken);

    public Task CommitTransactionAsync(CancellationToken cancellationToken = default) =>
        _context.Database.CommitTransactionAsync(cancellationToken);

    public Task RollbackTransactionAsync(CancellationToken cancellationToken = default) =>
        _context.Database.RollbackTransactionAsync(cancellationToken);
}