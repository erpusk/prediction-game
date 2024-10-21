<template>
    <EditGameEvent :game-event-id="gameEventId"  :prediction-game-id="predictionGameId"/>
  </template>
  
  <script setup lang="ts">
  import { useRoute } from 'vue-router';

  const route = useRoute();
  const gameEventId = parseInt(route.params.eventId.toString()); 
  const predictionGameId = parseInt(route.params.predictionGameId.toString())
  </script>