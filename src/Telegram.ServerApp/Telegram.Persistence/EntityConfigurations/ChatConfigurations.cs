using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.EntityConfigurations;

public class ChatConfigurations : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasOne(chatRoom => chatRoom.FirstUser)
               .WithMany()
               .HasForeignKey(chatRoom => chatRoom.FirstUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(chatRoom => chatRoom.SecondUser)
               .WithMany()
               .HasForeignKey(chatRoom => chatRoom.SecondUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(chatRoom => chatRoom.Messages)
               .WithOne(message => message.ChatRoom)
               .HasForeignKey(message => message.ChatRoomId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(chatRoom => chatRoom.LastMessage)
               .WithMany()
               .HasForeignKey(chatRoom => chatRoom.LastMessageId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}