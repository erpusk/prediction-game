<template>
    <AddGameEvent :prediction-game-id="predictionGameId" />
  </template>
  
  <script setup lang="ts">
  import { useRoute } from 'vue-router';
  
  const route = useRoute();
  const predictionGameId = Number(route.params.predictionGameId);
  </script>