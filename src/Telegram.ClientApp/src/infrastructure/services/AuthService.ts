import AuthApiClient from "../http/AuthApiClient";
import type { SignInModel } from "../models/SignInModel";
import router from "../router";
import LocalStorageService from "./LocalStorageService";

class AuthService {
    async signIn(params: SignInModel) {
        try {
            const response = await AuthApiClient.singIn(params);

            LocalStorageService.set('accessToken', response.data.token);
            LocalStorageService.set('me', response.data.user);

            router.push({ name: 'Home' });
        }
        catch (error) {
            console.error(error)
        }
    }
}

export default new AuthService();