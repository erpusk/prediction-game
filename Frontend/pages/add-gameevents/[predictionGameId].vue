<template>
    <AddGameEvent :prediction-game-id="predictionGameId" />
  </template>
  
  <script setup lang="ts">
  import { useRoute } from 'vue-router';

  definePageMeta({ middleware: 'auth' });
  
  const route = useRoute();
  const predictionGameId = Number(route.params.predictionGameId); 
  </script>