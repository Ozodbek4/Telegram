<template>
    <div class="max-w-xs p-4 rounded-lg shadow-xl" :class="isMe ? 'justify-end bg-sky-200 ml-auto' : 'justify-start bg-white mr-auto'">
        <p class="text-sm">{{ message.body }}</p>
        <div v-if="isMe" class="flex justify-end text-gray-500 text-xs">
            <span class="ml-1">{{ message.isSeen ? 'seen' : 'sended' }}</span>
        </div>
    </div>
</template>

<script lang="ts" setup>
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import type { User } from '../models/User';
import type { Message } from '../models/Message';
import { computed } from 'vue';

const me = LocalStorageService.get<User>('me');
const proms = defineProps<{ message: Message }>();
const isMe = computed(() => proms.message.senderId === me?.id);
// const receiver = computed(() => isMe.value ? proms.message.receiver : proms.message.sender);
</script>