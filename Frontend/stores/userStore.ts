import { defineStore } from 'pinia';
import { useStorage } from '@vueuse/core';
import type { AppUser } from '~/types/appUser';

export const useUserStore = defineStore('user', () => {
    const api = useApi();
    const user = ref<AppUser | null>(null);
    const token = ref<string | null>(null);

    const isAuthenticated = computed(() => !!token.value)
    const loadUser = async () => {
        try {
            const userData = await api.customFetch<AppUser>('User')
            user.value = userData;
        } catch(error) {
            console.error("Error loading user: ", error)
        }
    };

    const mockUser = { id: 1, name: 'Maksim', email: 'test@domain.ee' };
    const mockToken = 'fake-jwt-token';
  

    const login = async (credentials: { email: string; password: string }) => {
      try {
        await new Promise((resolve) => setTimeout(resolve, 500)); 

        if (credentials.email === 'test@domain.ee' && credentials.password === '1234') {
  
         user.value = mockUser;
         token.value = mockToken; 
          localStorage.setItem('token', mockToken);

        } else {
          throw new Error('Vale kasutajanimi või parool');
        }
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
            const response = await api.customFetch<{ user: AppUser; token: string}>('Register', {
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