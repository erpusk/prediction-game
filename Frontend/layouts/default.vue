<template>
  <div>
    <button 
      v-if="$route.path !== '/' && $route.path !== '/login' && $route.path !== '/register'"
      @click="goToMainPage" 
      class="fixed top-4 left-4 p-4 rounded-md flex items-center">
      <i class="fas fa-home text-gray-500" style="font-size: 28px"></i>
    </button>
    
    <div v-if="$route.path !== '/' && $route.path !== '/login'" class="button-group fixed bottom-4 left-4">
      <button class="btn-primary" @click="goBack">Back</button>
    </div>

    <NuxtPage />  
  </div>
</template>

<script setup lang="ts">
import { useRouter, useRoute } from 'vue-router';

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
.btn-primary {
  padding: 20px 60px;
  font-size: 18px;
  font-weight: bold;
  color: #ffffff;
  background-color: #d84141;
  border-radius: 80px;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s;
}
.btn-primary:hover {
  background-color: #4d6bac;
}
</style>