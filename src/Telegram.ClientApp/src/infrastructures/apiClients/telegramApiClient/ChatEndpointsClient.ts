import type { ApiClientBase } from "../apiClientBase/services/ApiClientBase";
import { Chat } from "@/modules/models/Chat";

export class ChatEndpointsClient{
    public client: ApiClientBase;

    constructor(client: ApiClientBase){
        this.client = client;
    }

    public async get() {
        const endpointUrl = "api/chat/chats";
        return await this.client.getAsync<Array<Chat>>(endpointUrl);
    }

    public async post(secondUserId: string) {
        const endpointUrl = "api/chat";
        return await this.client.postAsync<Chat>(endpointUrl, secondUserId);
    }

    public async delete(secondUserId: string){
        const endpointUrl = `api/chat/${secondUserId}`;
        
        return await this.client.deleteAsync(endpointUrl);
    }
}