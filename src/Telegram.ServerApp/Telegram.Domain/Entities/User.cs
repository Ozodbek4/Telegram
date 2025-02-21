using Telegram.Domain.Common.Entities;

namespace Telegram.Domain.Entities;

public class User : AuditableEntity
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool IsOnline { get; set; }
}