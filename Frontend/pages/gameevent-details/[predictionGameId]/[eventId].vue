<template>
    <GameEventDetails :eventId="gameEventId" :prediction-game-id="predictionGameId"/>
</template>

<script setup lang="ts">
    import { useRoute } from 'vue-router';

const route = useRoute();
const gameEventId = Number(route.params.eventId); 
const predictionGameId = Number(route.params.predictionGameId);
</script>