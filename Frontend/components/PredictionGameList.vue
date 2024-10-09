<template>
    <div class="p-6 bg-white rounded-lg shadow-lg">
      <button 
        @click="goToCreateNewPredictionGame" 
        class="absolute top-20 right-10 bg-gradient-to-r from-blue-500 to-blue-700 hover:from-blue-600 hover:to-blue-800 text-white font-semibold py-3 px-6 rounded-md shadow-md hover:shadow-lg transition duration-300 ease-in-out transform hover:scale-105">
        Create a new Prediction Game
      </button>
  
      
      <div v-if="predictionGames.length === 0">
        <h2 class="text-lg text-center text-gray-500">Prediction games are missing</h2>
      </div>
  
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black" >{{ title }}</h1>
        <UTable :rows="predictionGames" :columns="columns">
          <template #actions-data="{ row }">
            <button @click="deletePredictionGame(row)" class="flex items-center text-red-500 hover:text-red-700">
              <DeleteIconComponent />
            </button>
            <button @click="showEvents(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
              Show events
            </button>
            <button @click="goToAddGameEvent(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
              Add events
            </button>
          </template>
        </UTable>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { usePredictionGameStore } from '@/stores/stores';
  import DeleteIconComponent from '@/components/DeleteIconComponent.vue';
  import { useRouter } from 'vue-router';
  
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
    }
  ];
  
  const predictionGameStore = usePredictionGameStore();
  const { predictionGames } = storeToRefs(predictionGameStore);
  const gameEventStore = useGameEventsStore();
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

  const showEvents = (game: PredictionGame) => {
    const predictionGameId = game.id; // Get the predictionGameId from the clicked game
    router.push(`/gameevents/${predictionGameId}`);
  }

  const goToAddGameEvent = (game: PredictionGame) => {
    const predictionGameId = game.id;
    router.push(`/add-gameevents/${predictionGameId}`)
  }
  </script>  