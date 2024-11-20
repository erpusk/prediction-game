<template>
  <header class="bg-transparent" v-if="$route.path !== '/login' && $route.path !== '/register'">
    <div class="header-bg">
      <nav class="flex justify-between items-center p-4 bg-transparent text-white">
        <div class="flex items-center space-x-12">
          <nuxt-link to="/" class="logo-btn font-inter">
            <span>LOGO</span>
          </nuxt-link>

          <ul class="flex space-x-6 text-base font-medium">
            <li><nuxt-link to="/predictiongames" class="nav-link" :class="{ 'active': $route.path === '/predictiongames' }">My Prediction Games</nuxt-link></li>
            <li><nuxt-link to="/join-game" class="nav-link" :class="{ 'active': $route.path === '/join-game' }">Join a Prediction Game</nuxt-link></li>
          </ul>
        </div>

        <div v-if="userStore.isAuthenticated" class="flex items-center space-x-6">
          <span class="user-info font-inter">Hello, {{ userName }}!</span>
          <button class="btn-logout font-inter" @click="userStore.logout()">Logout</button>
        </div>
      </nav>

      <div class="hero-section relative h-screen bg-cover bg-center flex items-center justify-center text-white">
        <div class="overlay"></div>
        <div class="greeting-message relative text-center">
          <h1 class="text-6xl font-bold mb-6">Turn rivalries to memories!</h1>
          <p class="text-xl">Kick off your Prediction Game today.</p>
        </div>
      </div>
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

.header-bg {
  position: relative;
  width: 100%;
  height: 100vh;
  background-image: url('images/soccer-fans-cheering-team-monochrome (1).jpg');
  background-position: center top;
  background-size: cover;
  background-repeat: no-repeat;
  background-position-y: -100px;
  /* background-color: rgba(0, 0, 0, 0.5);
  background-blend-mode: overlay; */
}

.font-inter {
  font-family: 'Inter', sans-serif;
}

.nav-link {
  color: #fff;
  text-decoration: none;
  padding: 8px 16px;
  border-radius: 4px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.nav-link:hover {
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

/* .hero-section {
  position: relative;
  width: 100%;
  height: 100vh;
  background-image: url('images/soccer-fans-cheering-team-monochrome (1).jpg');
  background-position: center;
  background-size: cover;
  background-repeat: no-repeat;
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
} */

/* .overlay {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
} */

.greeting-message {
  z-index: 1;
  top: -100px;
}

</style>
