<template>
  <div id="page-container">
    <header v-if="$route.path !== '/login' && $route.path !== '/register'">
      <nav class="flex justify-between items-center p-4 bg-gray-900 text-white shadow-md">
        <!-- Logo and Navigation Links Section -->
        <div class="flex items-center space-x-12">
          <nuxt-link to="/" class="logo-btn font-inter">
            <span>LOGO</span>
          </nuxt-link>

          <ul class="flex space-x-6 text-base font-medium">
            <li><nuxt-link to="/predictiongames" class="nav-link" :class="{ 'active': $route.path === '/predictiongames' }">My Prediction Games</nuxt-link></li>
            <li><nuxt-link to="/join-game" class="nav-link" :class="{ 'active': $route.path === '/join-game' }">Join a Prediction Game</nuxt-link></li>
          </ul>
        </div>

        <!-- User Section (Hello + Logout) -->
        <div v-if="userStore.isAuthenticated" class="flex items-center space-x-6">
          <span class="user-info font-inter">Hello, {{ userName }}!</span>
          <button class="btn-logout font-inter" @click="userStore.logout()">Logout</button>
        </div>
      </nav>
    </header>

    <!-- Back Button for Non-Home Pages -->
    <div v-if="$route.path !== '/' && $route.path !== '/login' && $route.path !== '/register'" class="button-group fixed bottom-4 left-4 z-20">
      <button class="btn-primary" @click="goBack">Back</button>
    </div>

    <div id="content-wrap">
      <main class="content mt-0">
        <NuxtPage />
      </main>
    </div>

    <footer class="footer">
      <div class="footer-content p-4 text-white text-center">
        &copy; 2024 PredictionGames. All rights reserved.
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

const goBack = () => {
  router.go(-1); 
};
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');

#page-container {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

#content-wrap {
  flex-grow: 1;
  padding-bottom: 3rem;
}

.footer {
  position: relative;
  width: 100%;
  height: 3rem;
  background-color: #1F1F1F;
  color: white;
  text-align: center;
  z-index: 1;
  margin-top: auto;
}

footer .footer-content {
  padding: 16px;
  background-color: #1F1F1F;
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
</style>
