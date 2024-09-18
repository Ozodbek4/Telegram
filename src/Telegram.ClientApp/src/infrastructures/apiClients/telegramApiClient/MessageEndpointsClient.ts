import type { ApiClientBase } from "../apiClientBase/services/ApiClientBase";

export class MessageEndpointsClient {
    public client: ApiClientBase;

    constructor(client: ApiClientBase){
        this.client = client;
    }

    public async get(secondUserId: string) {
        const endpointUrl = `api/message/${secondUserId}`;

        return await this.client.getAsync(endpointUrl);
    }

    public async post(secondUserId: string, body: string) {
        const endpointUrl = `api/message?receiverId=${secondUserId}&body=${body}`

        return await this.client.postAsync(endpointUrl);
    }

    public async put(secondUserId: string, body: string){
        const endpointUrl = `api/message/${secondUserId}`

        return await this.client.putAsync(endpointUrl, body);
    }

    public async delete(messageId: string){
        const endpointUrl = `api/message/${messageId}`;

        return await this.client.deleteAsync(endpointUrl);
    }
}