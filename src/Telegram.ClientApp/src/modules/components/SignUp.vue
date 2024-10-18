<template>
    <div class="flex items-center justify-center min-h-screen bg-gray-100">
        <div class="w-full max-w-md bg-white shadow-lg rounded-lg p-6">
            <h2 class="text-2xl font-semibold text-center  pb-6">Create account</h2>
            <form @submit.prevent = "signUp">
                <div class="mb-4">
                    <label for="email" class="block text-sm font-medium text-gray-700 mb-1">Email</label>
                    <input type="email"
                        required
                        v-model="signUpDetails.emailAddress"
                        placeholder="Enter your email"
                        class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
                </div>
                <div class="mb-4">
                    <label for="firstname" class="block text-sm font-medium text-gray-700 mb-1">First Name</label>
                    <input type="text"
                        required
                        v-model="signUpDetails.firstName"
                        placeholder="Enter your first name"
                        class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
                </div>
                <div class="mb-4">
                    <label for="lastname" class="block text-sm font-medium text-gray-700 mb-1">Last Name</label>
                    <input type="text"
                        required
                        v-model="signUpDetails.lastName"
                        placeholder="Enter your last name"
                        class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
                </div>
                <div class="mb-4">
                    <label for="username" class="block text-sm font-medium text-gray-700 mb-1">Username</label>
                    <input type="text"
                        required
                        v-model="signUpDetails.userName"
                        placeholder="Enter your user name"
                        class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
                </div>
                <div class="mb-6">
                    <label for="password"class="block text-sm font-medium text-gray-700 mb-1">Password</label>
                    <input type="password"
                        required
                        v-model="signUpDetails.password"
                        placeholder="Enter your password"
                        class="w-full p-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-200">
                </div>
                <button type="submit"
                    class="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600 transition duration-200">
                    Sign Up
                </button>
            </form>
        </div>
    </div>
</template>

<script setup lang="ts">
import router from '@/infrastructures/router';
import { SignUpDetails } from '../models/SignUpDetails';
import SignIn from './SignIn.vue';


var signUpDetails = new SignUpDetails();

const signUp = () => {
    
    router.push({name:'Login'})
}

</script>