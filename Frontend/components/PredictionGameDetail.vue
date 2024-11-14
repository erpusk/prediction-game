<template>
    <div class="detail-page">
      <h2>{{ game?.predictionGameTitle }}</h2>
      <p class="border rounded-md p-2 text-center text-black"><strong>Privacy:</strong> {{ game?.privacy }}</p>
      <p><strong>Start Date:</strong> {{ game?.startDate }}</p>
      <p><strong>End Date:</strong> {{ game?.endDate }}</p>

    </div>
  </template>
  
  <script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { usePredictionGameStore } from '@/stores/stores';
import type { PredictionGame } from '~/types/predictionGame';

const props = defineProps<{
    predictionGameId: string | string[];
  }>();
const route = useRoute();
const predictionGameStore = usePredictionGameStore();
const game = ref<PredictionGame | null>(null);
    const event = ref({
    id: parseInt(props.id.toString(), 10),
    teamA: '',
    teamB: '',
    eventDate: '',
    predictionGameId: parseInt(props.predictionGameId.toString(), 10),
    teamAScore: 0,
    teamBScore: 0,
    isCompleted: false
  });
onMounted(async () => {
  const gameId = route.params.id as string;
  game.value = await predictionGameStore.getPredictionGameById(Number(gameId));
});
</script>
  
  <style scoped>
  .detail-page {
    padding: 20px;
    background-color: white;
    border-radius: 8px;
    max-width: 600px;
    margin: 20px auto;
  }
  </style>
  