<template>
    <div class="flex flex-col lg:flex-row h-screen">
        <!--  -->
        <div class="flex flex-col h-screen lg:w-1/4 bg-white border-r flex-shrink-0">
            <!-- Header -->
            <div class="p-4 border-b flex items-center justify-between">
                <h2 class="text-lg font-semibold">Chats</h2>
            </div>

            <!-- Search button -->
            <div class="p-4">
                <input type="text"
                    v-model="searchQuery"
                    placeholder="Search"
                    class="w-full p-2 border rounded-lg focus:outline-none focus:bg-gray-100 focus:ring-2 focus:ring-gray-400">
            </div>

            <!-- Chats list -->
            <div class="scrollbar-hidden overflow-y-auto">
                <div v-for="chat in chatLar"
                    :key="chat.id"
                    @click="selectChat"
                    class="p-4 hover:bg-gray-100 border-b cursor-pointer">
                    <div class="flex items-center">
                        <div class="w-12 h-12 rounded-full bg-gray-300 flex items-center justify-center">
                            <span class="text-white text-xl font-semibold">{{ chat.firstName.charAt(0) }}</span>
                        </div>
                        <div class="ml-4">
                            <h3 class="text-lg font-medium">{{ chat.firstName }} {{ chat.lastName }}</h3>
                            <p class="text-sm text-gray-500">{{ chat.message.body }}</p>
                        </div>
                        <div class="ml-auto text-xs text-gray-400">
                            {{ chat.lastMessageSendedAt }}
                        </div>
                    </div>

                </div>
            </div>

        </div>

        <!-- Chat window -->
        <div v-if="activeChat" class="flex flex-col w-full bg-white">
            <!-- Header -->
            <div class="flex items-center gap-4 p-4 border-b">
                <div class="w-10 h-10 bg-gray-300 rounded-full flex items-center justify-center">
                    <span class="text-white font-semibold text-xl">{{ activeChat.firstName.charAt(0) }}</span>
                </div>
                <p class="text-lg font-medium">{{ activeChat.firstName }} {{ activeChat.lastName }}</p>
                
            </div>

            <!-- Messages -->
            <div class="scrollbar-hidden flex flex-col-reverse overflow-y-auto p-6 bg-gray-50">
                <div class="flex flex-col gap-4">
                    <div v-for="mes in messages"
                    :key="mes.id"
                    :class="{'justify-end bg-white ml-auto': mes.receiverId == myId, 'justify-start bg-blue-500 mr-auto': mes.senderId == myId }"
                    class="max-w-xs p-4 rounded-lg shadow-lg">
                        <p class="text-sm">{{ mes.body }}</p>
                        <div v-if="mes.receiverId == myId" class="flex justify-end text-gray-500 text-xs">
                            <span class="ml-1">{{ mes.isSeen ? 'seen' : 'sended' }}</span>
                        </div>
                    </div>
            </div>
            </div>

            <!-- Input -->
            <div class="p-4 bg-white border-t flex items-center max-h-40">
                <input type="text"
                    v-model="newMessage.body"
                    placeholder="Write a message..."
                    class="w-full p-2 focus:outline-none"/>
                <button @click="sendMessage()" class="p-2 bg-blue-500 rounded hover:bg-blue-600 transition duration-200">Send</button>
            </div>
        </div>
    </div>
</template>
  
<script setup lang="ts">
import { Chat } from '@/modules/models/Chat';

var chatFirst = {
    id: "1",
    senderId: "1",
    recieverId: "2",
    firstName: "Ali",
    lastName: "Boburov",
    lastMessageSendedAt: "2mid ago",
    message: {
        body: "Salom"
    }
}
var chatSecond = {
    id: "2",
    senderId: "2",
    recieverId: "1",
    firstName: "Ali",
    lastName: "Boburov",
    lastMessageSendedAt: "2mid ago",
    message: {
        body: "Salom"
    }
}
chatSecond.id = "2"
chatSecond.firstName = "Ozodbek"
chatSecond.lastName = "Anvarjonov"
chatSecond.message.body = "Assalomu aleykum"
var chatLar = [chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond, chatFirst, chatSecond]

var searchQuery = "";


var activeChat = chatFirst
var showSidebar = false
const selectChat = () => {
    showSidebar = true
}

var message = {
    id: "1",
    senderId: "1",
    receiverId: "2",
    body: "Salommmmmmm",
    isSeen: true
}
var message1 = {
    id: "2",
    senderId: "2",
    receiverId: "1",
    body: "Salom",
    isSeen: false
}
var messages = [message1, message, message1,message, message, message1,message, message, message,message, message, message,message, message, message,message, message, message,message, message, message,message, message, message,message, message, message,message, message, message,message, message, message,message, message, message,message, message, message, message1]
const myId = "1";
const receiverId = "2"
var newMessage = {
    body: "",
}

function sendMessage() {
    if (newMessage.body == '')
        return;
    messages.push({
        id: "3",
        senderId: receiverId,
        receiverId: myId,
        body: newMessage.body,
        isSeen: false
    })
    activeChat.message.body = newMessage.body
    newMessage.body = ""
}

</script>

<style scoped>

    .scrollbar-hidden::-webkit-scrollbar{
        display: none;
    }

    .scrollbar-hidden {
        -ms-overflow-style: none;
        scrollbar-width: none;
    }
  /* Add any scoped styles for your chat component here */
</style>
  