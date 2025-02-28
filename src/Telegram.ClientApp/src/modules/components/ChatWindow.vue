<template>
    <!-- Head -->
    <div v-if="activeChat" class="flex flex-col w-full bg-white lg:flex-row h-screen">
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
import { computed, onMounted, ref, watch } from 'vue';
import MessageItem from './MessageItem.vue';
import MessageApiClient from '@/infrastructure/http/MessageApiClient';
import type { Message } from '../models/Message';
import type { CreateMessageModel } from '@/infrastructure/models/CreateMessageModel';

// define props and emits
const proms = defineProps<{ activeChat: ChatRoom | null }>();
const emit = defineEmits(['update:activechat']);

// reactive data
const newMessageBase = ref("");
const me = LocalStorageService.get<User>('me');
const isMe = computed(() => proms.activeChat!.firstUserId == me?.id);
const secondUser = computed(() => isMe.value ? proms.activeChat!.secondUser : proms.activeChat!.firstUser);
const messages = ref<Message[]>([]);

// fetch message
const messagesFunc = async () => {
    messages.value = (await MessageApiClient.getByChatRoomId(proms.activeChat!.id)).data;
}

// send message
const sendMessage = async (newMessage: string, activeChat: ChatRoom) => {
    if (proms.activeChat === null || newMessage.trim() == '')
        return;
    const mes = ref({} as CreateMessageModel);
    mes.value.body = newMessage;
    mes.value.senderId = me!.id;
    mes.value.receiverId = secondUser.value.id;

    const reponse = await MessageApiClient.post(mes.value);
    newMessageBase.value = "";
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
    console.log('done')
}

const handleEscKey = (event: KeyboardEvent) => {
    if (event.key === "Escape"){
        handleKeyBack();
    }
}

// watch active chat changes
watch(() => proms.activeChat, async () => {
    await messagesFunc();
}, { deep: true });

// fetch meesagesFunc
onMounted(() => {
    messagesFunc();
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