using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telegram.Domain.Entities;

namespace Telegram.Persistence.EntityConfiguration;

public class ChatConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.HasMany<Message>().WithOne().HasForeignKey(message => message.ChatId);

        builder.HasOne(chat => chat.FirstUser).WithMany().HasForeignKey(chat => chat.FirstUserId);

        builder.HasOne(chat => chat.SecondUser).WithMany().HasForeignKey(Chat => Chat.SecondUserId);

        builder.HasOne(chat => chat.LastMessage).WithOne().HasForeignKey<Chat>(chat => chat.LastMessageId);
    }
}