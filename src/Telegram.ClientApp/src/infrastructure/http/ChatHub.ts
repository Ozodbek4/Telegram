import * as signalR from "@microsoft/signalr";
import LocalStorageService from "../services/LocalStorageService";
import { ref } from "vue";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const connection = ref<HubConnection | null>(null);

const token = LocalStorageService.get<string>('accessToken');

connection.value = new HubConnectionBuilder()
    .withUrl(`https://localhost:7165/chat-hub?access_token=${token}`)
    .build();

export async function startConnection() {
    try {
        await connection.value?.start();
        console.log("Connected to singnalR");
    }
    catch (error) {
        console.error("Error with connecting singnalR:", error)
        setTimeout(startConnection, 50000);
    }
}

export async function sendMessage(userId: string, body: string) {
    try {
        await connection.value?.invoke("SendMessage", userId, body);
    }
    catch (error) {
        console.error("Error with sending message:", error);
    }
}

export async function onReceiveMessage(callback: (message: any) => void) {
    connection.value?.on("ReceiveMessage", callback);
}

export default connection;