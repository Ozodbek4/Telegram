<template>
    <div class="flex flex-col h-screen bg-white border-r flex-shrink-0 lg: flex flex-row h-screen">
        <!-- chats -->
        <div class="p-4 border-b flex center justify-center">
            <h2 class="text-lg font-semibold">Chats</h2>
        </div>

        <!-- search -->

        <div class="p-4">
            <input type="text"
                v-model="searchQuery"
                placeholder="Search"
                class="w-full p-2 border rounded-lg focus:outline-none focus:bg-gray-100 focus:ring-2 focus:ring-gray-400"
                @input="chatRoomsFunc(searchQuery)">
        </div>

        <!-- chat rooms -->
        <div class="scrollbar-hidden overflow-y-auto">
            <ChatItem
                v-for="chatRoom in chatRooms"
                :key="chatRoom.id"
                :chat="chatRoom"
                @select-chat="selectChat"/>
        </div>
    </div>
</template>

<script lang="ts" setup>
import ChatItem from '../components/ChatItem.vue';
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { User } from '../models/User';
import type { ChatRoom } from '../models/ChatRoom';
import { onMounted, ref } from 'vue';
import ChatRoomService from '@/infrastructure/services/ChatRoomService';
import { onReceiveMessage, onUserConnected, onUserDisconnected, startConnection } from '@/infrastructure/http/ChatHub';
import type { Message } from 'postcss';

const me = LocalStorageService.get<User>('me');
const chatRooms = ref<ChatRoom[]>([]);
const emit = defineEmits(['select-chat']);
const selectChat = (chat: ChatRoom) => emit('select-chat', chat);
const searchQuery = ref<string>('');

const chatRoomsFunc = async (search = '') => {
    try {
        const res = await ChatRoomService.getUserChatRooms(me!.id, search);
        chatRooms.value = [...res]; // Force Vue to recognize the update
    } catch (error) {
        // console.error('Error fetching chat rooms:', error);
    }
};

onReceiveMessage(async (message: Message) => {
    const chatRoomIndex = chatRooms.value.findIndex(c => c.id === message.chatRoomId);

    if (chatRoomIndex !== -1) {
        // Move chat to top
        const chatRoom = chatRooms.value.splice(chatRoomIndex, 1)[0];
        chatRooms.value.unshift(chatRoom);
    }

    // Fetch updated chat rooms (includes unread message counts)
    await chatRoomsFunc();
});


// Handle real-time user connection status
onUserConnected((userId: string) => {
    const chatRoom = chatRooms.value.find(c => c.secondUser.id.toString() === userId);
    if (chatRoom) {
        chatRoom.secondUser.isOnline = true;
    }
});

onUserDisconnected((userId: string) => {
    const chatRoom = chatRooms.value.find(c => c.secondUser.id.toString() === userId);
    if (chatRoom) {
        chatRoom.secondUser.isOnline = false;
    }
});

// Initialize WebSocket connection
onMounted(async () => {
    await chatRoomsFunc();
    await startConnection();
});
</script>

<style scoped>
.scrollbar-hidden::-webkit-scrollbar{
    display: none;
}

.scrollbar-hidden {
    -ms-overflow-style: none;
    scrollbar-width: none;
}
</style>