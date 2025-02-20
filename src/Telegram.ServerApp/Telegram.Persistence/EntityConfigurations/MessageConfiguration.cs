using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.EntityConfigurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasOne(m => m.Sender)
               .WithMany()
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Receiver)
               .WithMany()
               .HasForeignKey(m => m.ReceiverId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.ChatRoom)
               .WithMany(cr => cr.Messages)
               .HasForeignKey(m => m.ChatRoomId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}