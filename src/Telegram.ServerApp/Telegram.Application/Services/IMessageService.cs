using System.Linq.Expressions;
using Telegram.Application.Common.Models;
using Telegram.Domain.Entities;

namespace Telegram.Application.Services;

public interface IMessageService
{
    IQueryable<Message> Get(
        Expression<Func<Message, bool>>? expression = default,
        string[]? includes = null,
        bool asNoTracking = true
        );

    Task<Message> GetByIdAsync(
        long id,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<PaginationResult<Message>> GetByChatRoomIdAsync(
        long chatRoomId,
        PaginationParameters pagination,
        SortingParameters sorting,
        string? search = null,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default);

    Task<Message> UpdateAsync(Message message, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}