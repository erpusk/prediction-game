import { useUserStore } from '~/stores/userStore';

export default defineNuxtRouteMiddleware(async(to, from) => {
    const userStore = useUserStore();
    await userStore.loadUser();

    if (process.client) {
        const token = localStorage.getItem('token');
        userStore.isAuthenticated = !!token;
    } else {
        userStore.isAuthenticated = false;
    }

    if (to.path === '/login') {
        return;
    }

    if (!userStore.isAuthenticated) {
        return navigateTo("/login");
    }
});