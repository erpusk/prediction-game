<template>
    <PredictionGameDetail :id="predictionGameId"/>
</template>

<script setup lang="ts">
    import { useRoute } from 'vue-router';

const route = useRoute();
const predictionGameId = route.params.predictionGameId
</script>