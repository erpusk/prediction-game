<template>
    <div class="min-h-screen bg-white dark:bg-gray-900 flex justify-center items-start">
      <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg max-w-6xl w-full relative">
        
      <button 
        @click="showModal = true" 
        class="absolute top-6 right-5 btn-primary-large">
        Create a new Prediction Game
        </button>

      <div 
        v-if="showModal" 
        class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
        <AddPredictionGame @close="closeModal" />
      </div>

      <div v-if="predictionGames.length === 0" class="mt-6">
        <h2 class="text-lg text-center text-gray-500 ">Prediction games are missing</h2>
      </div>
  
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black dark:text-white" >{{ title }}</h1>
        <UTable :rows="formattedPredictionGames" :columns="columns">
          <template #actions-data="{ row }">
            <div class="flex items-center space-x-4">
              <button 
                @click="viewGameDetails(row)" 
                class="btn-primary-small" >
                View Details
              </button>
            <button 
        v-if="isParticipant(row)"
        @click="confirmLeaveGame(row)"
        class="btn-primary-small !bg-red-600 !hover:bg-red-700 !text-white">
        Leave Game
      </button>
     <button 
              v-if="isGameCreator(row)" 
              @click="confirmDelete(row)" 
              class="flex items-center text-red-500 hover:text-red-700">
              <DeleteIconComponent />
            </button>
          </div>
          </template>
        </UTable>
      </div>
    </div>
    <ConfirmationDialog
      :is-visible="showDialog"
      message="Erase game from your list?"
      @confirmed="deleteGame"
      @cancelled="cancelDelete"
    />
    <ConfirmationDialog
        :is-visible="showLeaveDialog"
        message="Are you sure you want to leave this game?"
        @confirmed="leaveGameConfirmed"
        @cancelled="cancelLeaveGame"
      />
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref } from 'vue';
  import { usePredictionGameStore } from '@/stores/stores';
  import DeleteIconComponent from '@/components/DeleteIconComponent.vue';
  import { useRouter } from 'vue-router';
  import { format } from 'date-fns';
  import type { PredictionGame } from '~/types/predictionGame';
  import { useUserStore } from '@/stores/userStore';
  import { storeToRefs } from 'pinia';
  import ConfirmationDialog from '@/components/ConfirmationDialog.vue';

  const showLeaveDialog = ref(false);
  const gameToLeave = ref<PredictionGame | null>(null);
  const showEraseButton = ref(false);
  const showDialog = ref(false);
  const currentGameToDelete = ref<PredictionGame | null>(null);

  const cancelLeaveGame = () => {
  showLeaveDialog.value = false;
  gameToLeave.value = null;
};
  const confirmLeaveGame = (game: PredictionGame) => {
  gameToLeave.value = game;
  showLeaveDialog.value = true;
};
  const confirmDelete = (game: PredictionGame) => {
  currentGameToDelete.value = game;
  showDialog.value = true;
};

const deleteGame = () => {
  if (currentGameToDelete.value) {
    deletePredictionGame();
    showEraseButton.value = false;
    console.log("Mäng on kustutatud");
    showDialog.value = false;
  }
};

const cancelDelete = () => {
  showEraseButton.value = false;
  showDialog.value = false;
};
  const predictionGameStore = usePredictionGameStore();
  const { predictionGames } = storeToRefs(predictionGameStore);

  const showModal = ref(false);

  const closeModal = () => {
  showModal.value = false; 
};
  
  defineProps<{ title: string }>();
  const leaveGame = async (game: PredictionGame) => {
  if (!game.uniqueCode) {
    return;
  }
  try {
    await predictionGameStore.leavePredictionGame(game.uniqueCode);
    predictionGames.value = predictionGames.value.filter(g => g.id !== game.id);
  } catch (error) {
  }
};


const isParticipant = (game: PredictionGame) => {
  const userId = userStore.user?.id;
  if (!userId || !game || !game.participants) return false;
  const found = game.participants.some(participant => participant.id === userId);
  return found && game.gameCreatorId !== userId;
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
  
  const router = useRouter();
  const userStore = useUserStore();
  const { user } = storeToRefs(userStore);
  
  const leaveGameConfirmed = async () => {
  if (!gameToLeave.value) return;
  try {
    await predictionGameStore.leavePredictionGame(gameToLeave.value.uniqueCode);
    predictionGames.value = predictionGames.value.filter(g => g.id !== gameToLeave.value?.id);
  } catch (error) {
    console.error('Error leaving the game:', error);
  } finally {
    showLeaveDialog.value = false;
    gameToLeave.value = null;
  }
};
  const formattedPredictionGames = computed(() => {
  return predictionGames.value.map(game => ({
    ...game,
    startDate: game.startDate ? format(new Date(game.startDate), 'dd.MM.yyyy') : '',
    endDate: game.endDate ? format(new Date(game.endDate), 'dd.MM.yyyy') : '',
  }));
});
  
const isGameCreator = (game: PredictionGame) => {
    const userId = userStore.user?.id;
    const isCreator = game.gameCreatorId === userId;
    return isCreator;
};
  
  onMounted(async () => {
  await predictionGameStore.loadPredictionGames();
  });
  
  const deletePredictionGame = () => {
  if (currentGameToDelete.value) {
    predictionGameStore.deletePredictionGame(currentGameToDelete.value);
    showDialog.value = false;
    currentGameToDelete.value = null;
  }
};

  const viewGameDetails = (game: PredictionGame) => {
    const predictionGameId = game.id;
    router.push(`/predictiongame-details/${predictionGameId}`);
  };
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
  background-color: #26547C;
  transform: scale(1.05);
}

.btn-primary-large:active {
  background-color: #26547C;
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
  background-color: #26547C;
  transform: scale(1.05);
}

.btn-primary-small:active {
  background-color: #26547C;
  transform: scale(1);
}
</style>