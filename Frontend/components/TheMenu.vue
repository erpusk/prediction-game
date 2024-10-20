<template>
  <div v-if="$route.path !== '/'" class="button-group fixed bottom-4 left-4">
    <button class="btn-primary" @click="goBack">Back</button>
  </div>
</template>

<script setup lang="ts">
import { useRouter, useRoute } from 'vue-router';

const router = useRouter();
const route = useRoute();

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