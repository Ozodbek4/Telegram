export class Message {
    public id!: string;

    public senderId!: string;

    public receiverId!: string;

    public chatId!: string;

    public createdDate!: TimeRanges;

    public body!: string;

    public isSeen!: boolean;
}