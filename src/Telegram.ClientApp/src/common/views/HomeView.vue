<script setup lang="ts">
import router from '@/infrastructures/router';
import { onMounted } from 'vue';
import { RouterView } from 'vue-router'


function intialMethod(){
    router.push({name: 'Login'})
}

onMounted(() => {
    intialMethod();
})

</script>

<template>
    <RouterView></RouterView>
</template>