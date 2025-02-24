import type { CreateChatRoomModel } from "../models/CreateChatRoomModel";
import axiosInstance from "./AxiosInstance";

class ChatRoomApiClient{
    async getById(id: number){
        return axiosInstance.get('api/chat-room/' + id);
    }

    async getUserChatRooms(userId: number){
        return axiosInstance.get('api/chat-room/user/' + userId);
    }

    async post(request: CreateChatRoomModel){
        return axiosInstance.post('api/chat-room/', request);
    }

    async deleteById(id: number){
        return axiosInstance.delete('api/chat-room/' + id);
    }
}

export default new ChatRoomApiClient();