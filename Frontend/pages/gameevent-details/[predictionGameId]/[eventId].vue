<template>
    <GameEventDetails :id="gameEventId" :prediction-game-id="predictionGameId"/>
</template>

<script setup lang="ts">
    import { useRoute } from 'vue-router';

    definePageMeta({ middleware: 'auth' });

const route = useRoute();
const gameEventId = route.params.eventId; 
const predictionGameId = route.params.predictionGameId
</script>