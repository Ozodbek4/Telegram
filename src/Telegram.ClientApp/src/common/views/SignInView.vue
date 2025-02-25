<template>
    <div class="flex items-center justify-center min-h-screen">
        <SignInForm @sign-in="signIn" @sign-up="signUp"/>
    </div>
</template>

<script lang="ts" setup>
import type { SignInModel } from '@/infrastructure/models/SignInModel';
import SignInForm from '../components/SignInForm.vue';
import AuthApiClient from '@/infrastructure/http/AuthApiClient';
import router from '@/infrastructure/router';

async function signIn(details: SignInModel) {
    try{
        const response = await AuthApiClient.singIn(details);
        console.log(response);
    }
    catch(error){
        console.error(error)
    }
}

async function signUp() {
    router.push({name: 'Sign up'});
}
</script>