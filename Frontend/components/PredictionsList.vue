<template>
  <div class="min-h-screen bg-white flex justify-center items-center p-6 dark:bg-gray-900">
    <div class="p-20 bg-white rounded-lg shadow-lg max-w-5x1 dark:bg-gray-800">
        <div v-if="predictions.length === 0">
          <h2 class="text-x1 text-center text-White ">No predictions have been added</h2>
        </div>
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black dark:text-white">{{ title }}</h1>
        <UTable :rows="predictions" :columns="columns"  />
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { onMounted } from 'vue';
const predictionsStore = usePredictionsStore();
const gameEventStore = useGameEventsStore();
const { predictions } = storeToRefs(predictionsStore);

const props = defineProps<{
  title: string;
  predictionGameId: string | string[];
  eventId: string | string[]
}>();

const event = await gameEventStore.loadSingleEvent(parseInt(props.eventId.toString()))

const columns = [
  {
    key: "endScoreTeamA",
    label: event.teamA + " end score",
  },
  {
    key: "endScoreTeamB",
    label: event.teamB + " end score",
  }
];

onMounted(() => {
  predictionsStore.loadPredictions(parseInt(props.eventId.toString()));
});




</script>