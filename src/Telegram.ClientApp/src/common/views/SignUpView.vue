<template>
    <div class="flex items-center justify-center min-h-screen bg-gray-100">
        <SignUpForm @sign-up="signUp"/>
    </div>
</template>

<script setup lang="ts">
import type { SignUpModel } from '@/infrastructure/models/SignUpModel';
import SignUpForm from '../components/SignUpForm.vue';
import AuthApiClient from '@/infrastructure/http/AuthApiClient';

async function signUp(details: SignUpModel){
    try{
        const response = await AuthApiClient.signUp(details);
        console.log(response);
    }
    catch (error){
        console.error('Sign up fiailed', error);
    }
}
</script>