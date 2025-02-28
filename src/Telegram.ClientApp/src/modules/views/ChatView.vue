<template>
    <div class="flex flex-col lg:flex-row h-screen">
        <ChatList
            v-if="!activeChat || isLargeScreen"
            @select-chat="selectChat"
            class="w-full lg:w-1/3"/>
        <div class="flex flex-col w-full bg-white">
            <ChatWindow
            v-model:activeChat="activeChat"
            v-if="activeChat"
            @update:activechat="activeChat = $event"/>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { computed, onMounted, onUnmounted, ref } from 'vue';
import ChatList from '../components/ChatList.vue';
import ChatWindow from '../components/ChatWindow.vue';
import type { ChatRoom } from '../models/ChatRoom';
import MessageApiClient from '@/infrastructure/http/MessageApiClient';
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { User } from '../models/User';

// reactive value
const activeChat = ref<ChatRoom | null>(null);
const windowWith = ref(window.innerWidth);

// responsive screen
const isLargeScreen = computed(() => windowWith.value >= 1024);

// Handle back button (also triggered by Escape key)
const handleBack = () => {
    if (!isLargeScreen.value) {
        activeChat.value = null;
    }
};

// Function to update window width
const updateWindowWidth = () => {
    windowWith.value = window.innerWidth;
};

// selected chat mark as seen
const selectChat = (chat: ChatRoom) => {
    activeChat.value = chat;
    const me = LocalStorageService.get<User>('me');
    MessageApiClient.putAsSeen(chat.id, me!.id)
};

// Event listeners for window resize
onMounted(() => {
    window.addEventListener('resize', updateWindowWidth);
});

onUnmounted(() => {
    window.removeEventListener('resize', updateWindowWidth);
});
</script>