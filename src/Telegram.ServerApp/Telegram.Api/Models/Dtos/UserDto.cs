namespace Telegram.Api.Models.Dtos;

public class UserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;
}