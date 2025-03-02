import type { ChatRoom } from "@/modules/models/ChatRoom";
import ChatRoomApiClient from "../http/ChatRoomApiClient";
import { PaginationParameters } from "../models/PaginationPrameters";
import UserApiClient from "../http/UserApiClient";
import LocalStorageService from "./LocalStorageService";
import type { User } from "@/modules/models/User";

class ChatRoomService {
    async getUserChatRooms(userId: number, search = ''): Promise<ChatRoom[]> {
        var params = new PaginationParameters({ SortBy: 'lastMessage', SortType: 'desc' });

        if (search == null || search === '') {
            return (await ChatRoomApiClient.getUserChatRooms(userId, params)).data;
        }

        return await this.getSearched(search);
    }

    private async getSearched(query: string): Promise<ChatRoom[]> {
        const params = new PaginationParameters({ PageSize: 10, search: query });
        let users = (await UserApiClient.get(params)).data as User[];
        const me = LocalStorageService.get<User>('me');
        users = users.filter(user => user.id !== me?.id);
        let chatRooms: ChatRoom[] = [];

        users.forEach(user => {
            let exist: ChatRoom = {
                id: 0,
                firstUserId: me!.id,
                firstUser: me!,
                secondUserId: user.id,
                secondUser: user,
                lastMessageId: 0,
                lastMessage: null,
                firstUserUnreadMessageCount: 0,
                secondUserUnreadMessageCount: 0,
            }

            chatRooms.push(exist);
        })

        return chatRooms;
    }
}

export default new ChatRoomService();