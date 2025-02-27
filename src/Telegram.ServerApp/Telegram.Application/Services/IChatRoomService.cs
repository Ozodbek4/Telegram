using System.Linq.Expressions;
using Telegram.Application.Common.Models;
using Telegram.Domain.Entities;

namespace Telegram.Application.Services;

public interface IChatRoomService
{
    IQueryable<ChatRoom> Get(
        Expression<Func<ChatRoom, bool>>? expression = default,
        string[]? includes = null,
        bool asNoTracking = true
        );

    Task<PaginationResult<ChatRoom>> GetAllAsync(
        PaginationParameters pagination,
        SortingParameters sorting,
        string? search = null,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<ChatRoom> GetByIdAsync(
        long id,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<PaginationResult<ChatRoom>> GetByUserIdAsync(
        long userId,
        PaginationParameters pagination,
        SortingParameters sorting,
        string? search = null,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<ChatRoom> GetByUsersIdAsync(
        long firstUserId,
        long secondUserId,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        );

    Task<ChatRoom> CreateAsync(ChatRoom chatRoom, CancellationToken cancellationToken = default);

    Task<ChatRoom> UpdateAsync(ChatRoom chatRoom, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);
}