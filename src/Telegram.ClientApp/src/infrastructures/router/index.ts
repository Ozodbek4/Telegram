import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/',
            name: 'Home',
            component: () => import('../../common/views/HomeView.vue')
        },
        {
            path: '/register',
            name: 'Register',
            component: () => import('../../modules/components/SignUp.vue')
        },
        {
            path: '/login',
            name: 'Login',
            component: () => import('../../modules/components/SignIn.vue')
        }
    ]
})

export default router;