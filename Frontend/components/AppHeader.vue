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

        <div v-if="userStore.isAuthenticated" >
          <div class="flex items-center space-x-6">
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
                  @click="userStore.logout"
                >
                  Logout
                </li>
              </ul>
            </div>
          </div>
        </div>
      </nav>
    </div>
  </header>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useUserStore } from '#imports';

const userStore = useUserStore();
const userName = computed(() => userStore.user?.userName);
const router = useRouter();
const isDropdownOpen = ref(false);


function toggleDropdown() {
  isDropdownOpen.value = !isDropdownOpen.value;
}

function closeDropdown() {
  isDropdownOpen.value = false;
}

function goToSettings(){
  router.push({ name: 'settings' });
    closeDropdown()
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

function encodeProfilePicture(array: any){
  return `data:image/jpeg;base64,${array}`;
}

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


button:focus {
  outline: none;
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