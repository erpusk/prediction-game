<template>
    <div class="detail-page">
      <h2 class="text-4xl font-semibold text-center mb-8 text-gray-800">{{ game.title }} details</h2>

      <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm">
          <p class="text-lg font-medium text-gray-700 text-center"><strong>Privacy:</strong> {{ game.privacy }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm">
          <p class="text-lg font-medium text-gray-700 text-center"><strong>Start date:</strong> {{ game.startDate }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm">
          <p class="text-lg font-medium text-gray-700 text-center"><strong>End date:</strong> {{ game.endDate }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm">
          <p class="text-lg font-medium text-gray-700 text-center"><strong>Creation date:</strong> {{ game.creationDate }}</p>
        </div>

        <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm">
          <p class="text-lg font-medium text-gray-700 text-center"><strong>Joined players:</strong> {{ game.participants }}</p>
        </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { usePredictionGameStore } from '@/stores/stores';

const props = defineProps<{
    id: string | string[];
  }>();
const route = useRoute();
const predictionGameStore = usePredictionGameStore();
const userStore = useUserStore();


const game = ref({
  id: parseInt(props.id.toString(), 10),
  startDate: '',
  endDate: '',
  creationDate: '',
  privacy: '',
  title: '',
  participants: [] as string[]
});

onMounted(async () => {
  const predictionGame = await predictionGameStore.loadPredictionGame(parseInt(props.id.toString()));
  //creationdate, startdate, enddate, privacy, predictiongametitle 
  game.value.startDate = predictionGame.startDate ? new Date(predictionGame.startDate).toLocaleDateString() : '';
  game.value.endDate = predictionGame.endDate ? new Date(predictionGame.endDate).toLocaleDateString() : '';
  game.value.creationDate = predictionGame.creationDate ? new Date(predictionGame.creationDate).toLocaleDateString() : '';
  game.value.privacy = predictionGame.privacy;
  game.value.title = predictionGame.predictionGameTitle

  const participantsUsernames = [] as string[];
  predictionGame.participants.forEach(async element => {
    const userName = await (await userStore.getUser(element.id)).userName

    participantsUsernames.push(userName)
  });

  game.value.participants = participantsUsernames
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
  