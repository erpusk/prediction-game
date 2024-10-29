import { useUserStore } from '~/stores/userStore';

export default defineNuxtRouteMiddleware(async (to, from) => {
    if (to.path === '/login' || to.path === '/register') {
        return;
    }
    const userStore = useUserStore();
    if (process.client) {
        await userStore.loadUser();

        const token = localStorage.getItem('token');
        if (!token || !userStore.user) {
            userStore.logout();
        }

        if (!userStore.isAuthenticated) {
            return navigateTo("/login");
        }
    }
});