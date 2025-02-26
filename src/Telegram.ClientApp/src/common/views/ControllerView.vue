<template></template>

<script lang="ts" setup>
import router from '@/infrastructure/router';
import LocalStorageService from '@/infrastructure/services/LocalStorageService';
import { onMounted } from 'vue';

onMounted(() => {
    const token = LocalStorageService.get('accessToken');
    if (token){
        router.push({name: 'Home'});
    }
    else{
        router.push({name: 'Sign in'});
    }
});
</script>