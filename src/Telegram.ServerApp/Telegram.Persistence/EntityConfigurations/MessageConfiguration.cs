using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.EntityConfigurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne(message => message.Sender)
               .WithMany()
               .HasForeignKey(message => message.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(message => message.Receiver)
               .WithMany()
               .HasForeignKey(message => message.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(message => message.ChatRoom)
               .WithMany(cr => cr.Messages)
               .HasForeignKey(message => message.ChatRoomId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}