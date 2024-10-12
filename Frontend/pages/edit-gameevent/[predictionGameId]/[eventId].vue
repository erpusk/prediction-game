<template>
    <EditGameEvent :game-event-id="gameEventId"  :prediction-game-id="predictionGameId"/>
  </template>
  
  <script setup lang="ts">
  import { useRoute } from 'vue-router';

  const route = useRoute();
  const gameEventId = route.params.eventId; 
  const predictionGameId = route.params.predictionGameId
  </script>