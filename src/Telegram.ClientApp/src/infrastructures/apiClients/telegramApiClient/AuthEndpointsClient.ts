import type { SignInDetails } from "@/modules/models/SignInDetails";
import type { ApiClientBase } from "../apiClientBase/services/ApiClientBase";
import type { IdentityToken } from "@/modules/models/IdentityToken";
import type { SignUpDetails } from "@/modules/models/SignUpDetails";
import { User } from "@/modules/models/User";
import type { head } from "node_modules/axios/index.cjs";

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
        const endpointUrl = "sign-up";
        return await this.client.postAsync(endpointUrl, signUpDetails);
    }

    public async singInAsync(signInDetails: SignInDetails) {
        const endpointUrl = "sign-in";
        return (await this.client.postAsync<IdentityToken>(endpointUrl, signInDetails));
    }
}