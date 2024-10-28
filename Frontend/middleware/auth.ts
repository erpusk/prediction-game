import { useUserStore } from '~/stores/userStore';

export default defineNuxtRouteMiddleware(async (to, from) => {
    const userStore = useUserStore();
    await userStore.loadUser();

    const token = localStorage.getItem('token');
    if (!token || !userStore.user) {
        userStore.logout();
    }

    if (to.path === '/login') {
        return;
    }

    if (!userStore.isAuthenticated) {
        return navigateTo("/login");
    }
});