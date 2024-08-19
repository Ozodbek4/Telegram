using Telegram.Application.Common.Helper;
using BC = BCrypt.Net.BCrypt;

namespace Telegram.Infrastructure.Common.Helper;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => BC.HashPassword(password);

    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
}