<template>
    <div>
      <button 
        @click="goToMainPage" 
        class="fixed top-4 left-4 p-4 rounded-md flex items-center">
        <i class="fas fa-home text-gray-500" style="font-size: 28px"></i>
      </button>
      <NuxtPage />  
    </div>
</template>

<script setup>
import { useRouter } from 'vue-router';
const router = useRouter();
const goToMainPage = () => {
  router.push('/');
};
</script>
