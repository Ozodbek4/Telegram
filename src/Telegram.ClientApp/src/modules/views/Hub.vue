<script setup lang="ts">
import { ref, onMounted, onUnmounted } from "vue";
import { startConnection, sendMessage, onReceiveMessage } from "../../infrastructure/http/ChatHub";

const messages = ref<{ user: string; body: string }[]>([]);
const newMessage = ref("");

onMounted(async () => {
    await startConnection();
    onReceiveMessage((message: any) => {
        messages.value.push(message);
    });
});

const handleSend = async () => {
    await sendMessage("12345", newMessage.value); // Replace with actual userId
    newMessage.value = "";
};

onUnmounted(() => {
    // Optional: Clean up listeners
});
</script>

<template>
    <div>
        <ul>
            <li v-for="(msg, index) in messages" :key="index">{{ msg.user }}: {{ msg.body }}</li>
        </ul>
        <input v-model="newMessage" placeholder="Type a message..." />
        <button @click="handleSend">Send</button>
    </div>
</template>
