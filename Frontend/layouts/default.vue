<template>
  <NuxtLayout>
    <button 
      @click="goToMainPage" 
      class="fixed top-4 left-4 bg-blue-500 text-white py-2 px-4 rounded-md hover:bg-blue-700">
      Main Page
    </button>
  </NuxtLayout>
</template>

<script setup>
import { useRouter } from 'vue-router';

const router = useRouter();

const goToMainPage = () => {
  router.push('/');
};
</script>


<style scoped>
  .fixed {
    position: fixed;
  }
  .top-4 {
    top: 1rem;
  }
  .left-4 {
    left: 1rem;
  }
  .bg-blue-500 {
    background-color: #3490dc;
  }
  .hover\:bg-blue-700:hover {
    background-color: #2779bd;
  }
  .text-white {
    color: white;
  }
  .py-2 {
    padding-top: 0.5rem;
    padding-bottom: 0.5rem;
  }
  .px-4 {
    padding-left: 1rem;
    padding-right: 1rem;
  }
  .rounded-md {
    border-radius: 0.375rem;
  }
</style>
