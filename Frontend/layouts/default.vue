<template>
  <div>
    <header v-if="$route.path !== '/login' && $route.path !== '/register'">
      <nav class="flex justify-between items-center p-4 bg-gray-800 text-white">
        <!-- Logo and Navigation Links Section -->
        <div class="flex items-center space-x-8">
          <nuxt-link to="/" class="logo-btn">
            <span>LOGO</span>
          </nuxt-link>

          <ul class="flex space-x-6 text-base font-medium">
            <li><nuxt-link to="/predictionGames" class="nav-link" :class="{ 'active': $route.path === '/predictionGames' }">My Prediction Games</nuxt-link></li>
            <li><nuxt-link to="/join-game" class="nav-link" :class="{ 'active': $route.path === '/join-game' }">Join a Prediction Game</nuxt-link></li>
          </ul>
        </div>

        <!-- User Section (Hello + Logout) -->
        <div v-if="userStore.isAuthenticated" class="flex items-center space-x-6">
          <span class="user-info">Hello, {{ userName }}!</span>
          <button class="btn-logout" @click="userStore.logout()">Logout</button>
        </div>
      </nav>
    </header>

    <!-- Back Button for Non-Home Pages -->
    <div v-if="$route.path !== '/' && $route.path !== '/login' && $route.path !== '/register'" class="button-group fixed bottom-4 left-4">
      <button class="btn-primary" @click="goBack">Back</button>
    </div>

    <main>
      <NuxtPage />
    </main>

    <footer>
      <div class="p-4 bg-gray-800 text-white text-center">
        &copy; 2024 Your Company. All rights reserved.
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { useRouter, useRoute } from 'vue-router';

const userStore = useUserStore();
const userName = computed(() => userStore.user?.userName);

const router = useRouter();
const route = useRoute();

const goToMainPage = () => {
  router.push('/');
};

const goBack = () => {
  router.go(-1); 
};
</script>

<style scoped>
.nav-link {
  color: #fff;
  text-decoration: none;
  padding: 8px 16px;
  border-radius: 4px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.nav-link:hover {
  /* background-color: rgba(255, 255, 255, 0.2); */
  color: #4CAF50;
  transform: scale(1.05);
}

.nav-link.active {
  color: #4CAF50;
  font-weight: 700;
}

.logo-btn {
  font-size: 1.5rem;
  font-weight: 700;
  color: white;
  text-transform: uppercase;
  letter-spacing: 1px;
  padding: 8px 16px;
  transition: color 0.3s ease-in-out;
}

.logo-btn:hover {
  color: #4CAF50;
}

nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  width: 100%;
}

.flex.items-center.space-x-8 {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  gap: 10px;
}

.btn-primary {
  padding: 16px 32px;
  font-size: 16px;
  font-weight: bold;
  color: #ffffff;
  background-color: #d84141;
  border-radius: 50px;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s;
}
.btn-primary:hover {
  background-color: #4d6bac;
}

.user-info {
  color: #fff;
  font-size: 1rem;
  font-weight: 500;
  transition: color 0.3s ease;
}

.btn-logout {
  padding: 9px 18px;
  font-size: 16px;
  font-weight: bold;
  color: #ffffff;
  background-color: #e74c3c;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s;
}
.btn-logout:hover {
  background-color: #c0392b;
  transform: scale(1.05);
}

button:focus {
  outline: none;
}
</style>
