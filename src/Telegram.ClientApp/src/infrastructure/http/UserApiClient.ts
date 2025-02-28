import type { PaginationParameters } from "../models/PaginationPrameters";
import type { SignInModel } from "../models/SignInModel";
import type { UpdateUserModel } from "../models/UpdateUserModel";
import axiosInstance from "./AxiosInstance";

class UserApiCleint {
    async getById(id: number) {
        return axiosInstance.get(`api/user/${id}`);
    }

    async get(params: PaginationParameters) {
        return axiosInstance.get(`api/user`, { params });
    }

    async post(request: SignInModel) {
        return axiosInstance.post(`api/user`, request);
    }

    async put(request: UpdateUserModel) {
        return axiosInstance.put(`api/user/${request.id}`, request);
    }

    async deleteById(id: number) {
        return axiosInstance.delete(`api/user/${id}`);
    }
}

export default new UserApiCleint();