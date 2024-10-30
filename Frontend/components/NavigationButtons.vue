<template>
    <div v-if="!isOnHomePage && !isOnLoginPage" class="button-group fixed bottom-4 left-4">
      <button class="btn-primary" @click="goBack">Back</button>
      <button @click="goToMainPage" class="btn-primary">
        <i class="fas fa-home text-gray-500" style="font-size: 28px"></i>
      </button>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useRouter, useRoute } from 'vue-router';
  
  const router = useRouter();
  const route = useRoute();
  
  const isOnHomePage = route.path === '/';
  const isOnLoginPage = route.path === '/login';
  
  const goBack = () => {
    router.go(-1); 
  };
  
  const goToMainPage = () => {
    router.push('/');
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