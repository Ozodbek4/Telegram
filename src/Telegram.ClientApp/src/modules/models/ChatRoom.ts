import type { Message } from "./Message";
import type { User } from "./User";

export interface ChatRoom {
    id: number;

    firstUserId: number;
    firstUser: User;

    secondUserId: number;
    secondUser: User;

    lastMessageId: number;
    lastMessage: Message | null;

    firstUserUnreadMessageCount: number;
    secondUserUnreadMessageCount: number;
}