<template>
    <div class="scrollbar-hidden overflow-y-auto">
        <ChatItem
            v-for="chatRoom in chatRooms"
            :key="chatRoom.id"
            :chat="chatRoom"/>
    </div>
</template>

<script lang="ts" setup>
import ChatRoomApiClient from '@/infrastructure/http/ChatRoomApiClient';
import ChatItem from '../components/ChatItem.vue';
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { User } from '../models/User';
import type { ChatRoom } from '../models/ChatRoom';
import { onMounted, ref } from 'vue';

const me = LocalStorageService.get<User>('me');
const chatRooms = ref<ChatRoom[]>([]);

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