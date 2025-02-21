using AutoMapper;
using System.Linq.Expressions;
using Telegram.Application.Common.Exceptions;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.UnitOfWorks;

namespace Telegram.Infrastructure.Services;

public class ChatRoomService(IUnitOfWork unitOfWork, IMapper mapper) : IChatRoomService
{
    public IQueryable<ChatRoom> Get(
        Expression<Func<ChatRoom, bool>>? expression = null,
        string[]? includes = null,
        bool asNoTracking = true
        )
    {
        return unitOfWork.ChatRooms.SelectAsQueryable(expression, includes, asNoTracking);
    }

    public async Task<ChatRoom> GetByIdAsync(
        long id,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = await unitOfWork.ChatRooms.SelectAsync(entity => entity.Id == id, includes, asNoTracking, cancellationToken)
            ?? throw new NotFoundException(nameof(ChatRoom), id);

        return exists;
    }

    public async Task<IEnumerable<ChatRoom>> GetByUserIdAsync(
        long userId,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = await unitOfWork.ChatRooms
            .SelectAsEnumerableAsync(entity => (entity.FirstUserId == userId || entity.SecondUserId == userId) && !entity.IsDeleted,
                includes, asNoTracking, cancellationToken);

        return exists;
    }

    public async Task<ChatRoom> GetByUsersIdAsync(
        long firstUserId,
        long secondUserId,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exist = await unitOfWork.ChatRooms
            .SelectAsync(entity => (entity.FirstUserId == firstUserId && entity.SecondUserId == secondUserId)
                || (entity.FirstUserId == secondUserId && entity.SecondUserId == firstUserId),
                includes, asNoTracking, cancellationToken)
            ?? throw new NotFoundException(nameof(ChatRoom), firstUserId);

        return exist;
    }

    public async Task<ChatRoom> CreateAsync(ChatRoom chatRoom, CancellationToken cancellationToken = default)
        {

        if (chatRoom.FirstUserId == chatRoom.SecondUserId)
            throw new ArgumentIsNotValidException("Chat room can not be with user id.");

        var exist = await unitOfWork.ChatRooms
            .SelectAsync(entity => (entity.FirstUserId == chatRoom.FirstUserId && entity.SecondUserId == chatRoom.SecondUserId)
                || (entity.FirstUserId == chatRoom.SecondUserId && entity.SecondUserId == chatRoom.FirstUserId),
                cancellationToken: cancellationToken);
        if (exist is not null)
            throw new AlreadyExistException(nameof(chatRoom), chatRoom.FirstUserId);

        var created = await unitOfWork.ChatRooms.CreateAsync(chatRoom, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return created;
    }

    public async Task<ChatRoom> UpdateAsync(ChatRoom chatRoom, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.ChatRooms.SelectAsync(entity => entity.Id == chatRoom.Id && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(ChatRoom), chatRoom.Id);

        var mapped = mapper.Map(chatRoom, exist);

        await unitOfWork.ChatRooms.UpdateAsync(mapped, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapped;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.ChatRooms.SelectAsync(entity => entity.Id == id && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(ChatRoom), id);

        await unitOfWork.ChatRooms.DeleteAsync(exist, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}