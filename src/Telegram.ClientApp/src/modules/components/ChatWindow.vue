<template>
    <!-- Head -->
    <div v-if="activeChat" class="flex flex-col w-full bg-white lg:flex flex-row h-screen">
        <div class="flex items-center gap-4 p-4 border-b">
            <button @click="handleKeyBack" class="w-10 h-10 bg-white/70 hover:bg-white/90 backdrop-blur-md border border-gray-300 shadow-lg flex items-center justify-center rounded-full transition-all duration-300 ease-in-out hover:scale-105">
                <svg class="w-5 h-5 text-gray-700" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                    <path stroke-linecap="round" stroke-linejoin="round" d="M15 19l-7-7 7-7"></path>
                </svg>
            </button>
            <div class="w-10 h-10 bg-gray-300 rounded-full flex items-center justify-center">
                <span class="text-white font-semibold text-xl">{{ secondUser.userName.charAt(0) }}</span>
            </div>
            <p class="text-lg font-medium">{{ secondUser.firstName }}</p>
            <p class="text-sm" :class="secondUser.isOnline ? 'text-green-300': 'text-gray-300'">{{ secondUser.isOnline ? 'Online' : 'Offline' }}</p>
        </div>

        <!-- Messages -->
        <div class="scrollbar-hidden flex flex-col-reverse overflow-y-auto p-6 bg-green-50 h-full gap-3">
            <MessageItem v-for="message in messages" :key="message.id" :message="message"/>
            <p v-if="loadingMessages" class="text-center text-gray-500">Loading messages...</p>
        </div>

        <!-- Input -->
        <div class="p-4 bg-white border-t flex items-center">
            <input type="text" v-model="newMessageBase" @keydown="handleKeyPress($event, newMessageBase, activeChat)" placeholder="Write a message..." class="w-full p-2 focus:outline-none">
            <button @click="sendMessage(newMessageBase, activeChat)" class="p-2 bg-blue-500 rounded hover:bg-blue-600 transition duration-200">Send</button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { ChatRoom } from '../models/ChatRoom';
import type { User } from '../models/User';
import { computed, nextTick, onMounted, ref, watch } from 'vue';
import MessageItem from './MessageItem.vue';
import MessageApiClient from '@/infrastructure/http/MessageApiClient';
import type { Message } from '../models/Message';
import type { CreateMessageModel } from '@/infrastructure/models/CreateMessageModel';
import { onReceiveMessage, onSendMessage, startConnection } from '@/infrastructure/http/ChatHub';

// define props and emits
const props = defineProps<{ activeChat: ChatRoom | null }>();
const emit = defineEmits(['update:activechat']);

// reactive data
const newMessageBase = ref('');
const me = LocalStorageService.get<User>('me');
const isMe = computed(() => props.activeChat!.firstUserId == me?.id);
const secondUser = computed(() => isMe.value ? props.activeChat!.secondUser : props.activeChat!.firstUser);
const messages = ref<Message[]>([]);
const loadingMessages = ref(false);

//
const loadMessageHistory = async () => {
    if (!props.activeChat) return;
        loadingMessages.value = true;
    try {
        const response = await MessageApiClient.getByChatRoomId(props.activeChat.id);
        messages.value = response.data;
        await nextTick();
    }
    catch (error) {
        // console.error('Failed to load messages:', error);
    }
    finally {
        loadingMessages.value = false;
    }
};


// watch
watch(() => props.activeChat, async (newChat) => {
    if (newChat) {
        messages.value = [];
        await loadMessageHistory();

        if (isMe.value) {
            newChat.firstUserUnreadMessageCount = 0;
        } else {
            newChat.secondUserUnreadMessageCount = 0;
        }
    }
}, { immediate: true, deep: true });

onReceiveMessage((message: Message) => {
    if (message.chatRoomId === props.activeChat?.id) {
        messages.value.unshift(message);
    };
});

// send message
const sendMessage = async (newMessage: string, activeChat: ChatRoom) => {
    if (props.activeChat === null || newMessage.trim() == '')
        return;
    const mes = ref({} as Message);
    mes.value.body = newMessage;
    mes.value.senderId = me!.id;
    mes.value.receiverId = secondUser.value.id;
    mes.value.createdAt = new Date().toString();

    messages.value.unshift(mes.value);
    await onSendMessage(mes.value.receiverId.toString(), newMessage)
    newMessageBase.value = '';
}

// handle enter key pass
const handleKeyPress = (event: KeyboardEvent, newMessage: string, activeChat: ChatRoom) => {
    if (event.key === "Enter") {
        sendMessage(newMessage, activeChat);
    }
};

// handle back button click
const handleKeyBack = () => {
    emit('update:activechat', null);
}

const handleEscKey = (event: KeyboardEvent) => {
    if (event.key === "Escape"){
        handleKeyBack();
    }
}

// fetch meesagesFunc
onMounted(() => {
    startConnection();
    window.addEventListener("keydown", handleEscKey)
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