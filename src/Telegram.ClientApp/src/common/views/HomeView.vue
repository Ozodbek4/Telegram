<template>
    <form @sumbit.prevent = "signUp">
        <button type="submit">SignIn</button>
    </form>
</template>

<script setup lang="ts">
import router from '@/infrastructures/router';
import SignUp from '@/modules/components/SignUp.vue';

const signUp = () => {
    router.push({name: 'Register'});
}

</script>