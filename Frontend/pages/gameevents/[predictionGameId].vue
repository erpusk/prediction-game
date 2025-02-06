<template>
    <GameEventsList :predictionGameTitle="'Events in the Prediction Game'" :prediction-game-id="predictionGameId" />
  </template>
  
  <script setup lang="ts">
  import GameEventsList from '@/components/GameEventsList.vue'; 
  import { useRoute } from 'vue-router';
  
  const route = useRoute();
  const predictionGameId = parseInt(route.params.predictionGameId.toString());
  </script>