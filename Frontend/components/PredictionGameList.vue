<template>
    <div>
        <button 
        @click="goToCreateNewPredictionGame" 
        class="absolute top-10 right-0 bg-gradient-to-r from-blue-500 to-blue-700 hover:from-blue-600 hover:to-blue-800 text-white font-semibold py-2 px-4 rounded-md shadow-md hover:shadow-lg transition duration-300 ease-in-out transform hover:scale-105 m-4">
        Create a new Prediction Game
        </button>
      <h1 class="text-xl text-center">{{ title }}</h1>
  
      <div v-if="predictionGames.length === 0">
        <h2 class="text-x1 text-center">Prediction games are missing</h2>
      </div>
      <div v-else>
        <UTable :rows="predictionGames" :columns="columns">
            <template #actions-data="{ row }">
              <button @click="deletePredictionGame(row)">
                  <DeleteIconComponent />
              </button>
            </template>
        </UTable>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { usePredictionGameStore } from '@/stores/stores';
  import DeleteIconComponent from '@/components/DeleteIconComponent.vue';
import AddPredictionGame from './AddPredictionGame.vue';
  
  defineProps<{ title: string }>();
  
  const columns = [
    {
      key: "predictionGameTitle",
      label: "Title",
    },
    {
      key: "privacy",
      label: "Privacy",
    },
    {
      key: "startDate",
      label: "Start Date",
    },
    {
      key: "endDate",
      label: "End Date",
    },
    {
      key: "actions",
      label: "Actions",
    },
  ];
  
  const predictionGameStore = usePredictionGameStore();
  const { predictionGames } = storeToRefs(predictionGameStore);

  const router = useRouter();

    const goToCreateNewPredictionGame = () => {
    router.push('/add-predictionGame');
    };
  
  onMounted(() => {
    predictionGameStore.loadPredictionGames();
  });
  
  const deletePredictionGame = (game: PredictionGame) => {
    predictionGameStore.deletePredictionGame(game);
  };
  </script>
  