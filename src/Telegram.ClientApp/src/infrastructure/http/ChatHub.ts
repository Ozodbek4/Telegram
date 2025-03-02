import * as signalR from "@microsoft/signalr";
import LocalStorageService from "../services/LocalStorageService";

const token = localStorage.getItem("accessToken");

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7165/chat-hub", {
        accessTokenFactory: () => `Bearer ${token}`,
        headers: {
            "Content-Type": "application/json",
        },
    })
    .withAutomaticReconnect()
    .build();;

export async function startConnection() {
    try {
        await connection.start();
        console.log("Connected to singnalR");
    }
    catch (error) {
        console.error("Error with connecting singnalR:", error)
        setTimeout(startConnection, 50000);
    }
}

export async function sendMessage(userId: string, body: string) {
    try {
        await connection.invoke("SendMessage", userId, body);
    }
    catch (error) {
        console.error("Error with sending message:", error);
    }
}

export async function onReceiveMessage(callback: (message: any) => void) {
    connection.on("ReceiveMessage", callback);
}

export default connection;