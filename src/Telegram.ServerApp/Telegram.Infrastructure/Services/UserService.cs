using AutoMapper;
using System.Linq.Expressions;
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
    public IQueryable<User> GetUsers(Expression<Func<User, bool>>? expression = null, string[]? includes = null, bool asNoTracking = true)
    {
        return unitOfWork.Users.SelectAsQueryable(expression, includes, asNoTracking);
    }
    
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

        exists.Password = await passwordHasherService.HashPassword(user.Password);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exists;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var exists = await GetByIdAsync(user.Id, asNoTracking: false, cancellationToken: cancellationToken);
        exists.FirstName = user.FirstName;
        exists.LastName = user.LastName;
        exists.UserName = user.UserName;
        exists.IsOnline = user.IsOnline;

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return exists;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var exits = await GetByIdAsync(id, asNoTracking: false, cancellationToken);

        await unitOfWork.Users.DeleteAsync(exits, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}