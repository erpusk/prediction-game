<template>
    <div class="min-h-screen bg-white dark:bg-gray-900 flex justify-center items-start">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg max-w-4xl w-full relative">
      
      <div class="relative">
        <button 
          v-if="isGameCreator"
          @click="showModal = true" 
          class="btn-primary-large absolute top-0 right-5">
          Add bonus question
        </button>
          <AddBonusQuestion
            v-if="showModal" 
            :predictionGameId="props.predictionGameId" 
            @close="closeModal" />

      <div class="bonus-questions">
        <h2 class="text-4xl font-semibold text-center mb-8 text-gray-800 dark:text-white">
          Bonus questions
        </h2>
  
          <ul class="space-y-2">
            <li v-for="(question, index) in bonusQuestions" :key="index" class="p-2 bg-gray-100 rounded-lg dark:bg-gray-600">
              <span class="font-semibold dark:text-white">{{ question.question }}</span>
            </li>
          </ul>
      </div>
      </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import { usePredictionGameStore } from '@/stores/stores';
  import type { BonusQuestion } from '~/types/bonusQuestion';

  const isGameCreator = ref(false);
  const userStore = useUserStore();
  const predictionGameStore = usePredictionGameStore();
  const userId = userStore.user?.id;
  const predictionGameCreatorId = ref<number | null>(null);
  const showModal = ref(false);
  
  const closeModal = () => {
  showModal.value = false; 
  };
  
  
  const props = defineProps<{
    predictionGameId: number;
  }>();
  
  const bonusQuestions = ref<BonusQuestion[]>([]);
  
  onMounted(async () => {
    const gameId = props.predictionGameId;
    bonusQuestions.value = await predictionGameStore.getPredictionGameBonusQuestions(gameId);

    const predictionGame = await predictionGameStore.getPredictionGameById(props.predictionGameId);
    if (predictionGame) {
        predictionGameCreatorId.value = predictionGame.gameCreatorId;
        isGameCreator.value = userId === predictionGameCreatorId.value;
    }
  });
  </script>
  
  <style scoped>
  .bonus-questions {
    padding: 20px;
    background-color: white;
    border-radius: 8px;
    max-width: 600px;
    margin: 20px auto;
  }
  
  .dark .bonus-questions {
    background-color: #1f2937;
  }

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

</style>
  