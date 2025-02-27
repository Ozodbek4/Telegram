import type { CreateMessageModel } from "../models/CreateMessageModel";
import type { UpdateMessageModel } from "../models/UpdateMessageModel";
import axiosInstance from "./AxiosInstance";

class MessageApiClient {
    async getById(id: number) {
        return axiosInstance.get(`api/message/${id}`);
    }

    async getByChatRoomId(chatRoomId: number) {
        return axiosInstance.get(`api/message/chat-room/${chatRoomId}`);
    }

    async post(request: CreateMessageModel) {
        return axiosInstance.post(`api/message`, request);
    }

    async put(request: UpdateMessageModel) {
        return axiosInstance.put(`api/message/${request.id}`, request);
    }

    async putAsSeen(chatRoomId: number, userId: number) {
        return axiosInstance.put(`api/message/mark-as-seen/${chatRoomId}/${userId}`)
    }

    async deleteById(id: number) {
        return axiosInstance.delete(`api/message/${id}`);
    }
}

export default new MessageApiClient();