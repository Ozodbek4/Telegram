import type { SignInDetails } from "@/modules/models/SignInDetails";
import type { ApiClientBase } from "../apiClientBase/services/ApiClientBase";
import type { IdentityToken } from "@/modules/models/IdentityToken";
import type { SignUpDetails } from "@/modules/models/SignUpDetails";
import { User } from "@/modules/models/User";

export class AuthEndpointsClient {
    public client: ApiClientBase;

    constructor(client: ApiClientBase){
        this.client = client;
    }

    public async getCurrentUser() {
        const endpointUrl = "me";
        return await this.client.getAsync<User>(endpointUrl);
    }

    public async signUpAsync(signUpDetails: SignUpDetails) {
        const endpointUrl = "signup";
        return await this.client.postAsync(endpointUrl, signUpDetails);
    }

    public async singInAsync(signInDetails: SignInDetails) {
        const endpointUrl = "signin";
        return (await this.client.postAsync<IdentityToken>(endpointUrl, signInDetails));
    }
}