<template>
  <div class="min-h-screen flex justify-center items-center p-6">
    <div class="p-20 bg-gray-600 rounded-lg shadow-lg max-w-5x1">
        <div v-if="predictions.length === 0">
          <h2 class="text-x1 text-center text-White">No predictions have been added</h2>
        </div>
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-white">{{ title }}</h1>
        <UTable :rows="predictions" :columns="columns"  />
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
import { onMounted } from 'vue';
const predictionsStore = usePredictionsStore();
const { predictions } = storeToRefs(predictionsStore);

const props = defineProps<{
  title: string;
  predictionGameId: string | string[];
  eventId: string | string[]
  }>();

const columns = [
  {
    key: "endScoreTeamA",
    label: "Team A end score",
  },
  {
    key: "endScoreTeamB",
    label: "Team B end score",
  }
];

onMounted(() => {
  predictionsStore.loadPredictions(parseInt(props.eventId.toString()));
});

</script>