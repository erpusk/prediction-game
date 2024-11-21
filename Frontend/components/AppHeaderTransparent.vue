<template>
  <header class="bg-transparent" v-if="$route.path !== '/login' && $route.path !== '/register'">
    <div class="header-bg">
      <nav class="flex justify-between items-center p-4 bg-transparent text-white">
        <div class="flex items-center space-x-12">
          <nuxt-link to="/" class="logo-btn font-inter">
            <span>LOGO</span>
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

      <div class="relative h-screen bg-cover bg-center flex items-center justify-center text-white">
        <div class="overlay"></div>
        <div class="greeting-message-rectangle">
          <h1 class="outline-text mb-4">TURN RIVALRIES TO MEMORIES</h1>
          <p class="text-2xl font-merriweather-400">Kick off your Prediction Game today!</p>
        </div>
        <div class="scroll-button absolute bottom-20 text-center w-full">
          <button class="btn-scroll font-merriweather-400" @click="scrollToTarget">
            ↓ Start predictions ↓
          </button>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { useUserStore } from '#imports';

const props = defineProps({
  scrollTarget: Object as PropType<HTMLElement | null>,
});

const userStore = useUserStore();
const userName = computed(() => userStore.user?.userName);
const route = useRoute();

const scrollToTarget = () => {
  if (props.scrollTarget) {
    props.scrollTarget.scrollIntoView({ behavior: "smooth" });
  }
};
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap');
@import url('https://fonts.googleapis.com/css2?family=Merriweather:wght@400;700;900&display=swap');

.font-merriweather-400 {
  font-family: 'Merriweather', sans-serif;
  font-weight: 400;
}

.header-bg {
  position: relative;
  width: 100%;
  height: 100vh;
  background-image: url('images/soccer-fans-cheering-gradient.png');
  background-position: center top;
  background-size: cover;
  background-repeat: no-repeat;
  background-position-y: -100px;
  background-color: rgba(0, 0, 0, 0.1);
  background-blend-mode: overlay;
}

.font-inter {
  font-family: 'Inter', sans-serif;
}

.nav-link {
  color: #000000;
  text-decoration: none;
  font-weight: bold;
  padding: 8px 16px;
  border-radius: 4px;
  font-weight:700;
  transition: all 0.3s ease;
  text-transform: uppercase;
  align-items: center;
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
  color: rgb(0, 0, 0);
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
  background-color: rgba(255, 255, 255, 0.4);
  background-blend-mode: overlay;
  font-size: large;
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

.greeting-message-rectangle {
  z-index: 1;
  top: -100px;
  position: relative;
  display: inline-block;
  padding: 1.5rem 2.5rem;
  background-color: rgba(0, 0, 0, 0.7);
  text-align: center;
}
.outline-text {
  font-size: 4rem;
  font-weight: bold;
  color: transparent;
  -webkit-text-stroke: 2.5px white;
  text-transform: uppercase;
  text-align: center;
}

.scroll-button {
  z-index: 10;
  position: absolute;
  bottom: 100px;
  left: 50%;
  transform: translateX(-50%);
  text-align: center;
}

.btn-scroll {
  padding: 8px 18px;
  font-size: 20px;
  font-weight: bold;
  color: #000000;
  background-color: transparent;
  border-radius: 30px;
  border: none;
  cursor: pointer;
  transition: all 0.3s ease;
}

.btn-scroll:hover {
  background-color: #d3d3d3;
  transform: scale(1.05);
}

</style>
