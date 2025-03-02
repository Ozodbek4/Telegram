import axios, { AxiosError, type InternalAxiosRequestConfig, type AxiosResponse } from "axios";
import LocalStorageService from "../services/LocalStorageService";

const axiosInstance = axios.create({
    baseURL: "https://localhost:7165",
    headers: {
        "Content-Type": "application/json",
    },
});

axiosInstance.interceptors.request.use(
    (config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
        const token = LocalStorageService.get<string>('accessToken');

        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    (error: AxiosError) => {
        console.error("Request Error:", error);
        return Promise.reject(error);
    }
);

axiosInstance.interceptors.response.use(
    (response: AxiosResponse) => response,
    (error: AxiosError) => {
        if (error.response?.status === 401) {
            console.warn("Unauthorized! Redirecting to login...");
            LocalStorageService.remove('accessToken');
            window.location.href = "/sign-in";
        } else if (error.response?.status === 403) {
            console.warn("403 Forbidden - Access denied.");
            alert("You do not have permission to perform this action.");
        }

        console.error("Response Error:", error);
        return Promise.reject(error);
    }
);

export default axiosInstance;