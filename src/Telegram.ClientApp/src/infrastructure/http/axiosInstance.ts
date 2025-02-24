import axios from "axios";
import LocalStorageService from "../services/LocalStorageService";

const axiosInstance = axios.create({
    baseURL: "https://localhost:7165",
    headers: {
        "Content-Type": "application/json",
    },
});

axiosInstance.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

axiosInstance.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            console.warn("Unauthorized! Redirecting to login...");
            LocalStorageService.remove("accessToken");
            window.location.href = "/login";
        }
        else if (error.response?.status === 403) {
            console.warn("403 Forbidden - Access denied.");
            alert("You do not have permission to perform this action.");
        }

        return Promise.reject(error);
    }
);

export default axiosInstance;