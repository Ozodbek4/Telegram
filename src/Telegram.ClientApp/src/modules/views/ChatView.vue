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

const activeChat = ref<ChatRoom | null>(null);
const selectChat = (chat: ChatRoom) => {
    activeChat.value = chat;
};
</script>