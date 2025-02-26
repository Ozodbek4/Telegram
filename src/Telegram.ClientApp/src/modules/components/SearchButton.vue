<template>
    <div class="p-4">
        <input type="text"
            v-model="searchQuery"
            placeholder="Search"
            class="w-full p-2 border rounded-lg focus:outline-none focus:bg-gray-100 focus:ring-2 focus:ring-gray-400">
    </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue';

const searchQuery = ref<String>('');
</script>