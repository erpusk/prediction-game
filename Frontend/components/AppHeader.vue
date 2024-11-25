<template>
  <header class="bg-gray-900" v-if="$route.path !== '/login' && $route.path !== '/register'">
    <div class="normal-header">
      <nav class="flex justify-between items-center p-4 bg-gray-900 text-white shadow-md">
        <div class="flex items-center space-x-12">
          <nuxt-link to="/" class="logo-btn font-inter">
            <i class='fas fa-futbol text-black bg-white' style='font-size:30px'></i>
            <span>     LOGO</span>
          </nuxt-link>

          <ul class="flex space-x-4 text-base font-medium">
            <li><nuxt-link to="/predictiongames" class="nav-link" :class="{ 'active': $route.path === '/predictiongames' }">My Prediction Games</nuxt-link></li>
            <li><nuxt-link to="/join-game" class="nav-link" :class="{ 'active': $route.path === '/join-game' }">Join a Prediction Game</nuxt-link></li>
          </ul>
        </div>

        <div v-if="userStore.isAuthenticated" class="flex items-center space-x-6">
          <span class="user-info font-inter">Hello, {{ userName }}!</span>
          <button class="btn-logout font-inter" @click="userStore.logout()">Logout</button>
        </div>
      </nav>
    </div>
  </header>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { useUserStore } from '#imports';

const userStore = useUserStore();
const userName = computed(() => userStore.user?.userName);
const route = useRoute();
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

.normal-header {
  position: relative;
  width: 100%;
  background-color: #1F1F1F;
}

.font-inter {
  font-family: 'Inter', sans-serif;
}

.nav-link {
  color: #ffffff;
  text-decoration: none;
  font-weight: bold;
  font-weight:700;
  padding: 8px 16px;
  border-radius: 4px;
  transition: all 0.3s ease;
  text-transform: uppercase;
}

.nav-link:hover {
  color: #5bb17c;
  transform: scale(1.05);
}

.nav-link.active {
  color: #5bb17c;
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
  color: #5bb17c;
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
  font-size: 1.2rem;
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