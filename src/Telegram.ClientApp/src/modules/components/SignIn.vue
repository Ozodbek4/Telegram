<template>
    <div class="flex items-center justify-center min-h-screen bg-gray-100">
        <div class="w-full max-w-md bg-white shadow-lg rounded-lg p-6">
            <h2 class="text-2xl font-semibold text-center pb-6">Enter your account</h2>
            <form @submit.prevent = signIn>
                <div class="mb-4">
                    <label for="text" class="block text-sm font-medium text-gray-700 mb-1">User name</label>
                    <input type="text"
                        placeholder="Enter your user name"
                        required
                        v-model="signInDetails.userName"
                        class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
                </div>
                <div class="mb-6">
                    <label for="password" class="block text-sm font-medium text-gray-700 mb-1">Password</label>
                    <input type="password"
                        placeholder="Enter your password"
                        required
                        v-model="signInDetails.password"
                        class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
                </div>
                <button type="submit" class="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600 transition duration-200 mb-4">Login</button>
            </form>
            <form @submit.prevent = signUp class="mt-auto text-end">
                <button class="text-blue-400 hover:text-blue-600">Create account</button>
            </form>
        </div>
    </div>
</template>

<script setup lang="ts">
import router from '@/infrastructures/router';
import { SignInDetails } from '../models/SignInDetails';

var signInDetails = new SignInDetails()

const signIn = () => {
    router.push({name: "Home"})
}

const signUp = () => {
    router.push({name: "Register"})
}

const res = router
</script>