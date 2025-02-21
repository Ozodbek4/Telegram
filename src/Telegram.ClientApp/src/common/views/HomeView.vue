<script setup lang="ts">
import router from '@/infrastructures/router';
import { onMounted } from 'vue';
import { RouterView } from 'vue-router'
import client from '@/services/TelegramApiClientService'
import axios, { type AxiosResponse } from 'axios';
import type { User } from '@/modules/models/User';
import telegramApiClient from '@/services/TelegramApiClientService';

async function intialMethod(){
    
    // const me = (await axios.get('https://localhost:7029/me'));
    
    
   
    
        router.push({name: 'Login'})
    
}

onMounted(() => {
    intialMethod();
})

</script>

<template>
    <RouterView></RouterView>
</template>