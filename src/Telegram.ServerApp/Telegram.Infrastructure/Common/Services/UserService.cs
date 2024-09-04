using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Telegram.Application.Common.Helper;
using Telegram.Application.Common.Services;
using Telegram.Domain.Entities;
using Telegram.Persistence.Repositories.Interfaces;

namespace Telegram.Infrastructure.Common.Services;

public class UserService(IUserRepository userRepository, IPasswordHasher passwordHasher) : IUserService
{
    public IEnumerable<User> Get(Expression<Func<User, bool>>? predicate = null, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        userRepository.Get(predicate, asNoTracking, cancellationToken);

    public async ValueTask<User?> GetByUserNameAsync(string userName, bool asNoTracking, CancellationToken cancellationToken = default) =>
        await userRepository.Get(cancellationToken: cancellationToken)
            .FirstOrDefaultAsync(entity => entity.UserName == userName);

    public ValueTask<User?> GetByIdAsync(Guid id, bool asNoTracking = false, CancellationToken cancellationToken = default) =>
        userRepository.GetByIdAsync(id, asNoTracking, cancellationToken);

    public ValueTask<User> CreateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        if (!IsUniqueEmail(entity.EmailAddress))
            throw new ArgumentException("Email is already registered");

        if (!IsUniqueUserName(entity.UserName))
            throw new ArgumentException("Username is already registered");

        entity.Password = passwordHasher.Hash(entity.Password);

        return userRepository.CreateAsync(entity, saveChanges, cancellationToken);
    }

    public async ValueTask<User> UpdateAsync(User entity, bool saveChanges = true, CancellationToken cancellationToken = default)
    {
        var foundEntity = await userRepository.GetByIdAsync(entity.Id, true, cancellationToken) ??
            throw new ArgumentNullException("User is not exists");
        if (!(foundEntity.EmailAddress == entity.EmailAddress && !IsUniqueEmail(entity.UserName)))
            throw new ArgumentException("Email is already registered");

        if (!(foundEntity.UserName == entity.UserName && !IsUniqueUserName(entity.UserName)))
            throw new ArgumentException("Username is already registered");

        entity.Password = passwordHasher.Hash(entity.Password);

        return await userRepository.UpdateAsync(entity, saveChanges, cancellationToken);
    }

    public ValueTask<User> DeleteByIdAsync(Guid id, bool saveChanges = true, CancellationToken cancellationToken = default) =>
        userRepository.DeleteByIdAsync(id, saveChanges, cancellationToken);

    private bool IsUniqueEmail(string emailAddress, CancellationToken cancellationToken = default) =>
        !userRepository.Get(asNoTracking: true, cancellationToken: cancellationToken).Any(user => user.EmailAddress == emailAddress);

    private bool IsUniqueUserName(string userName, CancellationToken cancellationToken = default) =>
        !userRepository.Get(asNoTracking: true, cancellationToken: cancellationToken).Any(user => userName == user.UserName);
}