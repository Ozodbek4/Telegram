<template>
    <div class="flex flex-col h-screen bg-white border-r flex-shrink-0 lg: flex flex-row h-screen">
        <div class="p-4 border-b flex items-center justify-between">
            <h2 class="text-lg font-semibold">Chats</h2>
        </div>

        <SearchButton/>

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
import ChatRoomApiClient from '@/infrastructure/http/ChatRoomApiClient';
import ChatItem from '../components/ChatItem.vue';
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { User } from '../models/User';
import type { ChatRoom } from '../models/ChatRoom';
import { onMounted, ref } from 'vue';
import SearchButton from './SearchButton.vue';

const me = LocalStorageService.get<User>('me');
const chatRooms = ref<ChatRoom[]>([]);
const emit = defineEmits(['select-chat']);
const selectChat = (chat: ChatRoom) => emit('select-chat', chat);

const chatRoomsFunc = async () => {
    var res = await ChatRoomApiClient.getUserChatRooms(me!.id);
    chatRooms.value = res.data;
}

onMounted(chatRoomsFunc);
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