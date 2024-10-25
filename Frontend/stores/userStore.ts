import { defineStore } from 'pinia';
import { useStorage } from '@vueuse/core';
import type { AppUser } from '~/types/appUser';

export const useUserStore = defineStore('user', () => {
    const api = useApi();
    const user = ref<AppUser | null>(null);
    const token = ref<string | null>(null);

    const isAuthenticated = computed(() => !!token.value)

    const loadUser = async () => {
      if(!token.value) return;
        try {
            const userData = await api.customFetch<AppUser>('User')
            user.value = userData;
        } catch(error) {
            console.error("Error loading user: ", error)
        }
    };

    const login = async (credentials: { email: string; password: string }) => {
      try {
        const response = await api.customFetch<{ user: AppUser; token: string }>('loginMethod', {
          method: 'POST',
          body: credentials,
        });
        
        user.value = response.user;
        token.value = response.token;

        localStorage.setItem('token', response.token);

      } catch (error) {
        console.error('Login failed: ', error);
      }
    };

    const setUser = (newUser: AppUser) => {
      user.value = newUser;  
    };
    
    const setToken = (newToken: string) => {
      token.value = newToken;  
    };

    const register = async (registrationData: { username: string; email: string; password: string}) => {
        try {
            const response = await api.customFetch<{ user: AppUser; token: string}>('registerMethod', {
                method: 'POST',
                body: registrationData,
            });
            user.value = response.user;
            token.value = response.token;

            localStorage.setItem('token', response.token)
        } catch(error) {
            console.error("Registration failed: ", error)
        }
    };

    const logout = () => {
        user.value = null;
        token.value = null;

        localStorage.removeItem('token');
    }

    return { user, token, isAuthenticated, setUser, setToken, loadUser, login, register, logout }
});