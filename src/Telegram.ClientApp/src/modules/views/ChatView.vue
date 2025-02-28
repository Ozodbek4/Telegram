<template>
    <div class="flex flex-col lg:flex-row h-screen">
        <ChatList @select-chat="selectChat"/>
        <ChatWindow v-if="activeChat" :activeChat="activeChat"/>
    </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue';
import ChatList from '../components/ChatList.vue';
import ChatWindow from '../components/ChatWindow.vue';
import type { ChatRoom } from '../models/ChatRoom';
import MessageApiClient from '@/infrastructure/http/MessageApiClient';
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { User } from '../models/User';

const activeChat = ref<ChatRoom | null>(null);
const selectChat = (chat: ChatRoom) => {
    activeChat.value = chat;
    const me = LocalStorageService.get<User>('me');
    MessageApiClient.putAsSeen(chat.id, me!.id)
};
</script>