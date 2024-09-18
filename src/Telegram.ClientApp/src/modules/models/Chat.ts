import type { Message } from "postcss";
import type { User } from "./User";

export class Chat {
    public id!: string;

    public FirstUserUnReadMessageCount!: number;

    public SecondUserUnReadMessageCount!: number;

    public LastMessage!: Message;

    public FirstUser!: User;

    public SecondUser!: User;
}