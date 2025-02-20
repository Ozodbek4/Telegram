using AutoMapper;
using Telegram.Application.Common.Exceptions;
using Telegram.Application.Common.Extensions;
using Telegram.Application.Common.Identity;
using Telegram.Application.Common.Models;
using Telegram.Application.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.UnitOfWorks;

namespace Telegram.Infrastructure.Services;

public class UserService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IPasswordHasherService passwordHasherService) : IUserService
{
    public Task<PaginationResult<User>> GetAllAsync(
        PaginationParameters pagination,
        SortingParameters sorting,
        string? search = null,
        bool asNoTracking = true,
        CancellationToken cancellationToken = default
        )
    {
        var exists = unitOfWork.Users.SelectAsQueryable(asNoTracking: asNoTracking);

        if (search is not null)
            exists = exists.Where(entity => entity.FirstName.ToLower().Contains(search.ToLower())
                || entity.LastName.ToLower().Contains(search.ToLower())
                || entity.UserName.ToLower().Contains(search.ToLower()));

        exists = exists
            .Where(entity => !entity.IsDeleted)
            .SortBy(sorting);

        return Task.FromResult(exists.ToPaginate(pagination));
    }

    public async Task<User> GetByIdAsync(long id, bool asNoTracking = true, CancellationToken cancellationToken = default)
    {
        var exists = await unitOfWork.Users.SelectAsync(entity => entity.Id == id && !entity.IsDeleted,
            asNoTracking: asNoTracking)
            ?? throw new NotFoundException(nameof(User), id);

        return exists;
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        var exists = await unitOfWork.Users.CreateAsync(mapper.Map<User>(user), cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exists;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var exists = await GetByIdAsync(user.Id, asNoTracking: false, cancellationToken: cancellationToken);
        var mapped = mapper.Map(user, exists);

        mapped.Password = await passwordHasherService.HashPassword(user.Password);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapped;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exits = await GetByIdAsync(id, asNoTracking: false, cancellationToken);

        await unitOfWork.Users.DeleteAsync(exits, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}