<template>
    <div class="min-h-screen bg-white flex justify-center items-start mt-10">
      <div class="bg-white rounded-lg shadow-lg max-w-6xl w-full relative">
      <button 
        @click="goToCreateNewPredictionGame" 
        class="absolute top-6 right-5 btn-primary-large">
        Create a new Prediction Game
      </button>
  
      
      <div v-if="predictionGames.length === 0" class="mt-6">
        <h2 class="text-lg text-center text-gray-500">Prediction games are missing</h2>
      </div>
  
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black" >{{ title }}</h1>
        <UTable :rows="formattedPredictionGames" :columns="columns">
          <template #actions-data="{ row }">
            <div class="flex items-center space-x-4">
            <button @click="showEvents(row)" class="btn-primary-small">
              Show events
            </button>
            <button v-if="isParticipant(row)" @click="leaveGame(row)" class="btn-primary-small">
  Leave Game
</button>

            <button 
              v-if="isGameCreator(row)" 
              @click="deletePredictionGame(row)" 
              class="flex items-center text-red-500 hover:text-red-700">
              <DeleteIconComponent />
            </button>
          </div>
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
  import type { PredictionGame } from '~/types/predictionGame';
  import { useUserStore } from '@/stores/userStore';
  
  defineProps<{ title: string }>();
  const leaveGame = async (game: PredictionGame) => {
  try {
    await predictionGameStore.leavePredictionGame(game.id);
    await predictionGameStore.loadPredictionGames();
  } catch (error) {
    console.error("Failed to leave the game:", error);
  }
};
const isParticipant = (game: PredictionGame) => {
  console.log("Participants:", game.participants);
  console.log("User ID:", user.value?.id);
  return (
    game.participants?.some(
      participant => participant.id === user.value?.id
    ) && !isGameCreator(game)
  );
};
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
      label: "",
    }
  ];
  
  const predictionGameStore = usePredictionGameStore();
  const { predictionGames } = storeToRefs(predictionGameStore);
  const router = useRouter();
  const userStore = useUserStore();
  const { user } = storeToRefs(userStore);

  const formattedPredictionGames = computed(() => {
  return predictionGames.value.map(game => ({
    ...game,
    startDate: game.startDate ? format(new Date(game.startDate), 'dd.MM.yyyy') : '',
    endDate: game.endDate ? format(new Date(game.endDate), 'dd.MM.yyyy') : '',
  }));
});
  
  const isGameCreator = (game: PredictionGame) => {
    return game.gameCreatorId === user.value?.id;
  };

  const goToCreateNewPredictionGame = () => {
    router.push('/add-predictionGame');
  };
  
  onMounted(async () => {
  await predictionGameStore.loadPredictionGames();
  });
  
  const deletePredictionGame = (game: PredictionGame) => {
    predictionGameStore.deletePredictionGame(game);
  };

  const showEvents = (game: PredictionGame) => {
    const predictionGameId = game.id; // Get the predictionGameId from the clicked game
    router.push(`/gameevents/${predictionGameId}`);
  }
  </script>  

<style scoped>
.btn-primary-large {
  padding: 9px 28px;
  font-size: 18px;
  font-weight: bold;
  color: #fff;
  background-color: #5bb17c;
  border-radius: 80px;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.3s;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.btn-primary-large:hover {
  background-color: #4d6bac;
  transform: scale(1.05);
}

.btn-primary-large:active {
  background-color: #3f6b96;
  transform: scale(1);
}

.btn-primary-small {
  padding: 5px 15px;
  font-size: 14px;
  font-weight: normal;
  color: #fff;
  background-color: #5bb17c;
  border-radius: 80px;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.3s;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.btn-primary-small:hover {
  background-color: #4d6bac;
  transform: scale(1.05);
}

.btn-primary-small:active {
  background-color: #3f6b96;
  transform: scale(1);
}
</style>