<template>
    <div class="w-full max-w-md bg-white shadow-xl rounded-lg p-6">
        <h2 class="text-2xl font-semibold text-center pb-6">Create your account</h2>
        <form @submit.prevent = "submit">
            <div class="mb-4">
                <label for="firstName" class="block text-sm font-medium text-gray-700 mb-1">First name</label>
                <input type="text" required v-model="form.firstName"
                    placeholder="Enter your first name"
                    class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
            </div>
            <div class="mb-4">
                <label for="lastName" class="block text-sm font-medium text-gray-700 mb-1">Last name</label>
                <input type="text" required v-model="form.lastName"
                    placeholder="Enter your last name"
                    class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
            </div>
            <div class="mb-4">
                <label for="userName" class="block text-sm font font-medium text-gray-700 mb-1">User name</label>
                <input type="text" required v-model="form.userName"
                    placeholder="Enter your user name"
                    class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
            </div>
            <div class="mb-6">
                <label for="password" class="block text-sm font font-medium text-gray-700 mb-1">Password</label>
                <input type="password" required v-model="form.password"
                    placeholder="Enter your password"
                    class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
            </div>
            <button type="submit" class="w-full bg-blue-500 text-white p-2 rounded hover:bg-blue-600 transition duration-1000">
                Sign Up
            </button>
        </form>
    </div>
</template>

<script lang="ts" setup>
import type { SignUpModel } from '@/infrastructure/models/SignUpModel';
import { ref } from 'vue';

const emit = defineEmits<{
    (event: 'signUp', details: SignUpModel): void
}>();

const form = ref({} as SignUpModel);

const submit = () => {
    emit('signUp', form.value);
};
</script>