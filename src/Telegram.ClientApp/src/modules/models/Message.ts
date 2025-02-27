import type { User } from "./User";

export interface Message {
    id: number;

    senderId: number;
    sender: User;

    receiverId: number;
    receiver: User;

    chatRoomId: number;
    body: string;
    isSeen: boolean;
    createdAt: string;
}