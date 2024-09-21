import { ApiClientBase } from "../apiClientBase/services/ApiClientBase";
import { AuthEndpointsClient } from "./AuthEndpointsClient";
import { ChatEndpointsClient } from "./ChatEndpointsClient";
import { MessageEndpointsClient } from "./MessageEndpointsClient";

export class TelegramApiClient {
    private readonly baseUrl: string;
    private readonly client: ApiClientBase;

    constructor() {
        this.baseUrl = "https://localhost:7029";
        this.client = new ApiClientBase({
            baseURL: this.baseUrl,
            withCredentials: true,
        })

        this.auth = new AuthEndpointsClient(this.client);
        this.chat = new ChatEndpointsClient(this.client);
        this.message = new MessageEndpointsClient(this.client);
    }

    public readonly auth: AuthEndpointsClient;
    public readonly chat: ChatEndpointsClient;
    public readonly message: MessageEndpointsClient;
}