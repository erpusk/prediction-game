<template>
    <PredictionGameInfo :predictionGameId="predictionGameId"/>
</template>

<script setup lang="ts">
    import { useRoute } from 'vue-router';

    const route = useRoute();
    const predictionGameId = Number(route.params.predictiongameId);
</script>