using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.EntityConfigurations;

public class ChatConfigurations : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasOne(cr => cr.FirstUser)
               .WithMany()
               .HasForeignKey(cr => cr.FirstUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cr => cr.SecondUser)
               .WithMany()
               .HasForeignKey(cr => cr.SecondUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(cr => cr.LastMessage)
               .WithMany()
               .HasForeignKey(cr => cr.LastMessageId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(cr => cr.Messages)
               .WithOne(m => m.ChatRoom)
               .HasForeignKey(m => m.ChatRoomId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}