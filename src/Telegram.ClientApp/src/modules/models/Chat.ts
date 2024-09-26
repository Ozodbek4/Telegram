import type { Message } from "./Message";
import type { User } from "./User";

export class Chat {
    public id!: string;

    public firstUserUnReadMessageCount!: number;

    public secondUserUnReadMessageCount!: number;

    public lastMessage!: Message;

    public firstUser!: User;

    public secondUser!: User;
}