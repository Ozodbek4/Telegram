<template>
    <div class="w-full  max-w-md bg-white shadow-xl rounded-lg p-6">
        <h2 class="text-2xl font-medium text-center pb-6">Enter your account</h2>
        <form @submit.prevent = "submit">
            <div class="mb-4">
                <label for="userName" class="block text-sm font-medium text-gray-700 mb-1">User name</label>
                <input type="text" id="userName" required
                    v-model="form.userName"
                    placeholder="Enter your user name"
                    class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
            </div>
            <div class="mb-4">
                <label for="password" class=" block text-sm font-medium text-gray-700 mb-1">Password</label>
                <input type="password" id="password" required
                    v-model="form.password"
                    placeholder="Enter your password"
                    class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
            </div>
            <button type="submit" class="w-full p-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-1000 mb-4">Sign In</button>
        </form>
        <div @click = "signUp" class="text-end">
            <button class="text-blue-400 hover:text-blue-600">Create your account</button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import type { SignInModel } from '@/infrastructure/models/SignInModel';
import { ref } from 'vue';

const emit = defineEmits<{
    (event: 'signIn', details: SignInModel): void,
    (event: 'signUp'): void
}>();

const form = ref({} as SignInModel);

const submit = () => {
    emit('signIn', form.value);
}

const signUp = () => {
    emit('signUp');
}
</script>