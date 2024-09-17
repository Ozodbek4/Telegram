import { ApiClientBase } from "@/infrastructures/apiClients/apiClientBase/services/ApiClientBase";
import { TelegramApiClient } from "@/infrastructures/apiClients/telegramApiClient/TelegramApiClient";
import { LocalStorageService } from "@/infrastructures/services/LocalStorageService";
import type { SignInDetails } from "../models/SignInDetails";
import type { SignUpDetails } from "../models/SignUpDetails";

export class AuthenticationService{
    private readonly telegramClient: TelegramApiClient;
    private readonly localStorage: LocalStorageService

    constructor() {
        this.telegramClient = new TelegramApiClient();
        this.localStorage = new LocalStorageService();
    }

    public hasAccessToken() {
        return this.localStorage.get('accessToken') !== null;
    }

    public isLoggined() {
        return this.telegramClient.auth.getCurrentUser() !== null;
    }

    public async signInAsync(signInDetails: SignInDetails) {
        const signInResponse = await this.telegramClient.auth.singInAsync(signInDetails);

        if (!signInResponse.isSuccess)
            return false;

        this.localStorage.set('accessToken', signInResponse.response?.accessToken);

        return true;
    }

    public async signUpAsync(signUpDetails: SignUpDetails) {
        return await this.telegramClient.auth.signUpAsync(signUpDetails);
    }
}