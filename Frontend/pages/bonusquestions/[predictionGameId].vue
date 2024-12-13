<template>
    <BonusQuestionList :prediction-game-id="Id"/>
</template>

<script setup lang="ts">
  import GameEventsList from '@/components/GameEventsList.vue'; 
  import { useRoute } from 'vue-router';
  
  const route = useRoute();
  const Id = parseInt(route.params.predictionGameId.toString());
  </script>