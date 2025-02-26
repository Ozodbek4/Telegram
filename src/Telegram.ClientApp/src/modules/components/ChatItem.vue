<template>
    <div @click="$emit('select-chat', chat)"
        class="p-4 hover:bg-gray-100 border-b cursor-pointer flex items-center">
        <div class="w-12 h-12 rounded-full bg-gray-300 flex items-center justify-center">
            <span class="text-white text-xl font-semibold">{{ secondUser.firstName.charAt(0) }}</span>
        </div>
        <div class="ml-4">
            <h3 class="text-lg font-medium">{{ secondUser.firstName + ' ' + secondUser.lastName}}</h3>
            <p class="text-sm text-gray-500">{{ chat.lastMessage?.body || 'No message yet' }}</p>
        </div>
        <div v-if="chat.firstUserUnreadMessageCount"
            class="min-w-5 h-5 rounded-full bg-blue-500 flex items-center justify-center">
            <span class="text-white text-base p-2">{{ chat.firstUserUnreadMessageCount }}</span>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { computed } from 'vue';
import type { ChatRoom } from '../models/ChatRoom';
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { User } from '../models/User';

const me = LocalStorageService.get<User>('me');
const proms = defineProps<{ chat: ChatRoom }>();
const isMe = computed(() => proms.chat.firstUserId === me?.id);
const secondUser = computed(() => isMe.value ? proms.chat.secondUser : proms.chat.firstUser);
</script>