import type { SignInModel } from "../models/SignInModel";
import type { SignUpModel } from "../models/SignUpModel";
import axiosInstance from "./AxiosInstance";

class AuthApiClient {
    async singIn(request: SignInModel) {
        return axiosInstance.post("api/auth/sing-in", request);
    }

    async signUp(request: SignUpModel) {
        return axiosInstance.post("api/user", request);
    }
}

export default new AuthApiClient();