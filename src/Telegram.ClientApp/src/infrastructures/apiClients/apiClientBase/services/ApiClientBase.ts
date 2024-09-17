import type { AxiosInstance } from "axios";

export class ApiClientBase{
    public readonly client!: AxiosInstance;

    public async getAsync<T>(url: string): Promise<T> {
        return (await this.client.get<T>(url)).data;
    }

    public async postAsync<T>(url: string, data?: any): Promise<T> {
        return (await this.client.post<T>(url, data)).data;
    }

    public async putAsync<T>(url: string, data?: any): Promise<T> {
        return (await this.client.put<T>(url, data)).data;
    }

    public async deleteAsync<T>(url: string): Promise<T> {
        return (await this.client.delete<T>(url)).data;
    }
}