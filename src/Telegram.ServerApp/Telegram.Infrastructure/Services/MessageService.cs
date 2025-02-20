using AutoMapper;
using System.Linq.Expressions;
using Telegram.Application.Common.Exceptions;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.UnitOfWorks;

namespace Telegram.Infrastructure.Services;

public class MessageService(IUnitOfWork unitOfWork, IMapper mapper) : IMessageService
{
    public IQueryable<Message> Get(
        Expression<Func<Message, bool>>? expression = null,
        string[]? includes = null,
        bool asNoTracking = true
        )
    {
        return unitOfWork.Messages.SelectAsQueryable(expression, includes, asNoTracking);
    }

    public async Task<Message> GetByIdAsync(
        long id,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exist = await unitOfWork.Messages.SelectAsync(entity => entity.Id == id && !entity.IsDeleted,
            includes, asNoTracking, cancellationToken)
            ?? throw new NotFoundException(nameof(Message), id);

        return exist;
    }

    public async Task<IEnumerable<Message>> GetByChatRoomIdAsync(
        long chatRoomId,
        string[]? includes = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = await unitOfWork.Messages
            .SelectAsEnumerableAsync(entity => entity.ChatRoomId == chatRoomId && !entity.IsDeleted,
                includes, asNoTracking, cancellationToken);

        return exists;
    }

    public async Task<Message> CreateAsync(Message message, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Messages.CreateAsync(message, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exist;
    }

    public async Task<Message> UpdateAsync(Message message, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Messages.SelectAsync(entity => entity.Id == message.Id && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(Message), message.Id);

        var mapped = mapper.Map(message, exist);

        await unitOfWork.Messages.UpdateAsync(mapped, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapped;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exist = await unitOfWork.Messages.SelectAsync(entity => entity.Id == id && !entity.IsDeleted)
            ?? throw new NotFoundException(nameof(Message), id);

        await unitOfWork.Messages.DeleteAsync(exist, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}