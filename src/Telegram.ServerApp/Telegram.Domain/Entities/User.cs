using Telegram.Domain.Common.Entities;

namespace Telegram.Domain.Entities;

public class User : SoftDeletedEntity
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}