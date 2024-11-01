<template>
  
    <div class="min-h-screen bg-white flex justify-center items-center ">
      <div class="bg-white rounded-lg shadow-lg max-w-5xl w-full">
      <button 
        @click="goToCreateNewPredictionGame" 
        class="absolute top-20 right-10 bg-gradient-to-r from-blue-500 to-blue-700 
        hover:from-blue-600 hover:to-blue-800 text-white font-semibold py-3 px-6 rounded-md shadow-md 
        hover:shadow-lg transition duration-300 ease-in-out transform hover:scale-105">
        Create a new Prediction Game
      </button>
  
      
      <div v-if="predictionGames.length === 0">
        <h2 class="text-lg text-center text-gray-500">Prediction games are missing</h2>
      </div>
  
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black" >{{ title }}</h1>
        <UTable :rows="formattedPredictionGames" :columns="columns">
          <template #actions-data="{ row }">
            <button @click="deletePredictionGame(row)" class="flex items-center text-red-500 hover:text-red-700">
              <DeleteIconComponent />
            </button>
            <button @click="showEvents(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
              Show events
            </button>
          </template>
        </UTable>
      </div>
    </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { usePredictionGameStore } from '@/stores/stores';
  import DeleteIconComponent from '@/components/DeleteIconComponent.vue';
  import { useRouter } from 'vue-router';
  import { format } from 'date-fns';
  
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
  const router = useRouter();

  const formattedPredictionGames = computed(() => {
  return predictionGames.value.map(game => ({
    ...game,
    startDate: game.startDate ? format(new Date(game.startDate), 'dd.MM.yyyy') : '',
    endDate: game.endDate ? format(new Date(game.endDate), 'dd.MM.yyyy') : '',
  }));
});
  
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
  </script>  