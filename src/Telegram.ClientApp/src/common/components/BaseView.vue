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
                <div v-for="chat in chats"
                    :key="chat.id"
                    @click="selectChat(chat)"
                    class="p-4 hover:bg-gray-100 border-b cursor-pointer">
                    <div class="flex items-center">
                        <div class="w-12 h-12 rounded-full bg-gray-300 flex items-center justify-center">
                            <span class="text-white text-xl font-semibold">{{ chat.secondUser.id == myId ?  chat.firstUser.userName.charAt(0) : chat.secondUser.userName.charAt(0) }}</span>
                        </div>
                        <div class="ml-4">
                            <h3 class="text-lg font-medium">{{  chat.secondUser.id == myId ?  chat.firstUser.firstName : chat.secondUser.firstName  }} {{ chat.secondUser.id == myId ?  chat.firstUser.lastName : chat.secondUser.lastName }}</h3>
                            <p class="text-sm text-gray-500">{{ chat.lastMessage.body }}</p>
                        </div>
                        <div class="ml-auto text-xs text-gray-400">
                            {{ chat.lastMessage.createdDate }}
                        </div>
                    </div>

                </div>
            </div>

        </div>

        <!-- Chat window -->
        <div v-if="activeChat !== null" class="flex flex-col w-full bg-white">
            <!-- Header -->
            <div class="flex items-center gap-4 p-4 border-b">
                <div class="w-10 h-10 bg-gray-300 rounded-full flex items-center justify-center">
                    <span class="text-white font-semibold text-xl">{{ activeChat.secondUser.id == myId ?  activeChat.firstUser.userName.charAt(0) : activeChat.secondUser.userName.charAt(0) }}</span>
                </div>
                <p class="text-lg font-medium">{{ activeChat.secondUser.id == myId ?  activeChat.firstUser.firstName : activeChat.secondUser.firstName  }}</p>
                
            </div>

            <!-- Messages -->
            <div class="scrollbar-hidden flex flex-col-reverse overflow-y-auto p-6 bg-gray-50 h-full">
                <div class="flex flex-col gap-4">
                    <div v-for="mes in showChatMessages(activeChat)"
                    :key="mes.id"
                    :class="{'justify-end bg-white ml-auto': mes.senderId == myId, 'justify-start bg-blue-500 mr-auto': mes.receiverId == myId }"
                    class="max-w-xs p-4 rounded-lg shadow-lg">
                        <p class="text-sm">{{ mes.body }}</p>
                        <div v-if="mes.receiverId == myId" class="flex justify-end text-gray-500 text-xs">
                            <span class="ml-1">{{ mes.isSeen ? 'seen' : 'sended' }}</span>
                        </div>
                    </div>
            </div>
            </div>

            <!-- Input -->
            <div class="p-4 bg-white border-t flex items-center">
                <input type="text"
                    v-model="newMessage"
                    placeholder="Write a message..."
                    class="w-full p-2 focus:outline-none"/>
                <button @click="sendMessage(newMessage, activeChat)" class="p-2 bg-blue-500 rounded hover:bg-blue-600 transition duration-200">Send</button>
            </div>
        </div>
    </div>
</template>
  
<script setup lang="ts">
import { Chat } from '@/modules/models/Chat';
import type { Ref } from 'vue';
import { ref } from 'vue';

type customMessage = {
    id: string,
    senderId: string,
    receiverId: string,
    chatId: string,
    body: string,
    createdDate:  string,
    isSeen: boolean
}
type customChat = {
    id: string,
    firstUserId: string,
    secondUserId: string,
    firstUser: {
        id: string,
        firstName: string,
        lastName: string,
        userName: string,
        emailAddress: string
    },
    secondUser: {
        id: string,
        firstName: string,
        lastName: string,
        userName: string,
        emailAddress: string
    },
    lastMessage: customMessage,
}
const myId = "1";
const newMessage = ref("");
const activeChat = ref<customChat|null>(null);
const showSidebar = ref(false)

const chatFirst: customChat = {
    id: "1",
    firstUserId: "1",
    secondUserId: "2",
    firstUser: {
        id: "1",
        firstName: "Ozodbek",
        lastName: "Anvarjonov",
        userName: "Ozodbek",
        emailAddress: "anvarjonovozodbek416@gmail.com"
    },
    secondUser: {
        id: "2",
        firstName: "Behruz",
        lastName: "Anvarjonov",
        userName: "Behruz",
        emailAddress: "anvarjonovbehruz777@gmail.com"
    },
    lastMessage: {
        id: "1",
        senderId: "1",
        receiverId: "2",
        chatId: "1",
        body: "Salom, Man Ozodbekman Behruzxon",
        createdDate:  "2days ago",
        isSeen: false
    },
}
const chatSecond : customChat = {
    id: "2",
    firstUserId: "1",
    secondUserId: "3",
    firstUser: {
        id: "1",
        firstName: "Ozodbek",
        lastName: "Anvarjonov",
        userName: "Ozodbek",
        emailAddress: "anvarjonovozodbek416@gmail.com"
    },
    secondUser: {
        id: "3",
        firstName: "Sarvinoz",
        lastName: "Akramova",
        userName: "Sarvinoz",
        emailAddress: "akramovasarvinoz@gmail.com"
    },
    lastMessage: {
        id: "2",
        senderId: "3",
        receiverId: "1",
        chatId: "2",
        body: "Qalaysan uka Man Sarivnozman. San Ozodbeksan",
        createdDate:  "1day ago",
        isSeen: true
    }
}

const chats = ref<customChat[]>([chatFirst, chatSecond])

const searchQuery = ref("");


const message : customMessage = {
    id: "3",
    senderId: "3",
    receiverId: "1",
    chatId: "2",
    body: "Qalaysan uka, yaxshimisan yetvoldimi Sarivnoz opang",
    createdDate:  "1day ago",
    isSeen: false
}
const message1: customMessage = {
    id: "4",
    senderId: "1",
    receiverId: "2",
    chatId: "1",
    body: "Qalaysan Behruz, yaxshimisa. Ozodbek akang",
    createdDate:  "1day ago",
    isSeen: true
}
const messages = ref<customMessage[]>([message, message1, message1, message])


function selectChat(chat:  customChat) {
    activeChat.value = chat
    showSidebar.value = true
}

function sendMessage(mes: string, chat: customChat) {
    if (chat === null || newMessage.value.trim() == '')
        return;
    var recId = chat.firstUserId
    if (chat.firstUserId == myId)
        recId = chat.secondUserId
    messages.value.push({
        id: "4",
        senderId: myId,
        receiverId: recId,
        chatId: chat.id,
        body: mes,
        createdDate:  "1min ago",
        isSeen: false
    })

    newMessage.value = ""
    console.log("MEssages is worked")
}
function showChatMessages(chat: customChat): customMessage[]
{
    const activeChatMessages = messages.value.filter(mes => mes.chatId === chat.id)
    
    return activeChatMessages;
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
  