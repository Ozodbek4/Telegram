import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'control',
            component: () => import('../../common/views/ControllerView.vue'),
        },
        {
            path: '/home',
            name: 'Home',
            component: () => import('../../modules/views/ChatView.vue'),
        },
        {
            path: '/sign-up',
            name: 'Sign up',
            component: () => import('../../common/views/SignUpView.vue'),
        },
        {
            path: '/sign-in',
            name: 'Sign in',
            component: () => import('../../common/views/SignInView.vue'),
        }
    ]
});

export default router;