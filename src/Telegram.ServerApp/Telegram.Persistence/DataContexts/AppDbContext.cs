using Telegram.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Telegram.Persistence.DataContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}