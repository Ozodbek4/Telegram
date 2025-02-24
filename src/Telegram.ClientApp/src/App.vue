<template>
</template>

<script lang="ts" setup>
import { onMounted } from 'vue';
import AuthApiClient from './infrastructure/http/AuthApiClient';
import ChatRoomApiClient from './infrastructure/http/ChatRoomApiClient';


async function fetchData() {
    const response = await AuthApiClient.singIn({userName: "Ozodbek", password: "Ozodbek"});
    console.log(response);

    const chatRoomResponse = await ChatRoomApiClient.getById(1);
    console.log(chatRoomResponse);
}

onMounted(fetchData);
</script>