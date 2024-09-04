using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.EntityConfiguration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne<User>().WithMany().HasForeignKey(message => message.SenderId);

        builder.HasOne<User>().WithMany().HasForeignKey(message => message.ReceiverId);
    }
}