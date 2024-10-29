<template>
    <PredictionsList  :prediction-game-id="predictionGameId" :event-id="gameEventId" :title="'Predictions that have been made'"/>
  </template>
  
  <script setup lang="ts">
  import { useRoute } from 'vue-router';
  
  const route = useRoute();
  const predictionGameId = route.params.predictionGameId; 
  const gameEventId = route.params.eventId;
  </script>