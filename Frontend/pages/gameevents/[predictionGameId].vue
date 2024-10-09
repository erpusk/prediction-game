<template>
    <GameEventsList :title="'Events that are in a prediction game'" :prediction-game-id="predictionGameId" />
  </template>
  
  <script setup lang="ts">
  import GameEventsList from '@/components/GameEventsList.vue'; // Adjust path as necessary
  import { useRoute } from 'vue-router';
  
  const route = useRoute();
  const predictionGameId = route.params.predictionGameId; // Get predictionGameId from route params
  </script>