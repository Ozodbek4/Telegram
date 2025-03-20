import * as signalR from "@microsoft/signalr";
import LocalStorageService from "../services/LocalStorageService";
import { ref } from "vue";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const connection = ref<HubConnection | null>(null);

const token = LocalStorageService.get<string>('accessToken');

connection.value = new HubConnectionBuilder()
    .withUrl(`https://chat.ozodbek4.uz/chat-hub?access_token=${token}`)
    .build();

export async function startConnection() {
    try {
        await connection.value?.start();
        // console.log("Connected to singnalR");
    }
    catch (error) {
        // console.error("Error with connecting singnalR:", error)
        setTimeout(startConnection, 50000);
    }
}

// Handle Online Status
export function onUserConnected(callback: (userId: string) => void) {
    connection.value?.on("UserConnected", callback);
}

export function onUserDisconnected(callback: (userId: string) => void) {
    connection.value?.on("UserDisconnected", callback);
}

export async function onSendMessage(userId: string, body: string) {
    try {
        await connection.value?.invoke("SendMessage", userId, body);
    }
    catch (error) {
        // console.error("Error with sending message:", error);
    }
}

export async function onReceiveMessage(callback: (message: any) => void) {
    connection.value?.on("ReceiveMessage", callback);
}

export default connection;