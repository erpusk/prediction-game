<template>
  <header class="bg-transparent" v-if="$route.path !== '/login' && $route.path !== '/register'">
    <div class="header-bg">
      <nav class="nav-bar flex justify-between items-center p-4 bg-transparent text-white">
        <div class="flex items-center space-x-12">
          <nuxt-link to="/" class="logo-btn font-inter">
            <i class='fas fa-futbol text-black bg-white' style='font-size:30px'></i>
            <span>     LOGO</span>
          </nuxt-link>

          <ul class="flex space-x-4 text-base font-xl">
            <li><nuxt-link to="/predictiongames" class="nav-link" :class="{ 'active': $route.path === '/predictiongames' }">My Prediction Games</nuxt-link></li>
            <li><nuxt-link to="/join-game" class="nav-link" :class="{ 'active': $route.path === '/join-game' }">Join a Prediction Game</nuxt-link></li>
          </ul>
        </div>

        <div v-if="userStore.isAuthenticated" class="flex items-center space-x-6">
          <span class="user-info font-inter">Hello, {{ userName }}!</span>
          <div class="relative dropdown-container">
          <button
                class="btn-settings font-inter"
                @click="toggleDropdown"
              >
                <img
                  v-if="userStore.user?.profilePicture"
                  :src="encodeProfilePicture(userStore.user?.profilePicture)"
                  alt="Profile Picture"
                  class="profile-picture"
                />
                <span v-else>...</span>
              </button>
              <!-- Dropdown menu -->
              <ul
                v-show="isDropdownOpen"
                class="absolute right-0 mt-2 w-48 bg-white border border-gray-300 rounded shadow-md text-black"
              >
                <li
                  class="px-4 py-2 hover:bg-gray-100 cursor-pointer"
                  @click="goToSettings"
                >
                  Settings
                </li>
                <li
                  class="px-4 py-2 hover:bg-gray-100 cursor-pointer"
                  @click="goToAccountDetails"
                >
                  Account
                </li>
                <li
                  class="px-4 py-2 hover:bg-gray-100 cursor-pointer"
                  @click="userStore.logout"
                >
                  Logout
                </li>
              </ul>
            </div>
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
const router = useRouter();
const isDropdownOpen = ref(false);

const scrollToTarget = () => {
  if (props.scrollTarget) {
    props.scrollTarget.scrollIntoView({ behavior: "smooth" });
  }
};

function encodeProfilePicture(array: any){
  return `data:image/jpeg;base64,${array}`;
}

function handleClickOutside(event: MouseEvent) {
  const dropdownElement = document.querySelector('.dropdown-container');
  if (dropdownElement && !dropdownElement.contains(event.target as Node)) {
    closeDropdown();
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside);
});

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside);
});

function goToSettings(){
  router.push({ name: 'settings' });
    closeDropdown()
}

function goToAccountDetails(){
  router.push({ name: 'account-details' });
    closeDropdown()
}

function toggleDropdown() {
  isDropdownOpen.value = !isDropdownOpen.value;
}

function closeDropdown() {
  isDropdownOpen.value = false;
}

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
  background-image: url('/images/soccer-fans-cheering-gradient.png');
  background-position: center top;
  background-size: cover;
  background-repeat: no-repeat;
  background-position-y: -100px;
  background-color: rgba(0, 0, 0, 0.0);
  background-blend-mode: overlay;
}

.font-inter {
  font-family: 'Inter', sans-serif;
}

.nav-bar {
  background-color: #131B23;
}

.nav-link {
  color: #ffffff;
  text-decoration: none;
  padding: 8px 16px;
  border-radius: 4px;
  font-weight:700;
  transition: all 0.3s ease;
  text-transform: uppercase;
  align-items: center;
  font-size: medium;
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
  color: rgb(255, 255, 255);
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
  background-color: rgba(255, 255, 255, 0.5);
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
  color: #ffffff;
  font-size: 1.2rem;
  font-weight: 500;
  transition: color 0.3s ease;
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
  background-color: #d3d3d34e;
  transform: scale(1.05);
}

.profile-picture {
  width: 40px; 
  height: 40px;
  border-radius: 50%; 
  object-fit: cover; 
  border: 2px solid #ddd; 
}

.profile-picture:hover {
  transform: scale(1.05); 
}

.btn-settings {
  display: flex; 
  align-items: center; 
  justify-content: center; 
  width: 40px; 
  height: 40px; 
  background-color: #6e7278; 
  border: none; 
  border-radius: 50%; 
  font-size: 1rem;
  cursor: pointer;
}

.btn-settings:hover {
  background-color: #8c8f93;
  transform: scale(1.05); 
}

.dropdown-container {
  position: relative; 
}

.dropdown-container ul {
  position: absolute;
  top: 100%; 
  right: 0;
  z-index: 1000; 
  background-color: white;
  border: 1px solid #ccc;
  border-radius: 0.25rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.dropdown-container ul li {
  padding: 0.5rem 1rem;
  cursor: pointer;
  white-space: nowrap; 
}

.dropdown-container ul li:hover {
  background-color: #f1f1f1;
}

</style>
