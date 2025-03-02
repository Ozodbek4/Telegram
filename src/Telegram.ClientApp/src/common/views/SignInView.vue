<template>
    <div class="flex items-center justify-center min-h-screen">
        <SignInForm @sign-in="signIn" @sign-up="signUp"/>
    </div>
</template>

<script lang="ts" setup>
import type { SignInModel } from '@/infrastructure/models/SignInModel';
import SignInForm from '../components/SignInForm.vue';
import router from '@/infrastructure/router';
import AuthService from '@/infrastructure/services/AuthService';

async function signIn(params: SignInModel) {
    await AuthService.signIn(params);
}

async function signUp() {
    router.push({name: 'Sign up'});
}
</script>