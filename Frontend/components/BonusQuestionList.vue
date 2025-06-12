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

        <div v-if="bonusQuestions.length === 0" class="text-center text-gray-500 dark:text-gray-400">
          No bonus questions added for this game
        </div>
          <ul v-else class="space-y-2">
            <li v-for="(question) in bonusQuestions" :key="question.id" class="p-2 bg-gray-100 rounded-lg dark:bg-gray-600 flex justify-between items-center ">
              <span class="font-semibold dark:text-white">{{ question.question }}</span>
              <div v-if="hasAnsweredMap[question.id] === true">
                    <button class="bg-gray-500 text-white rounded-[80px] px-4 py-1.5">
                      Already answered
                    </button>
              </div>
              <div v-else>
                <button 
                @click="showModalAddAnswer[question.id] = true" 
                class="btn-secondary">
                Answer
              </button>
              <AddAnswer
                v-if="showModalAddAnswer[question.id]" 
                :questionId="question.id"
                @close="closeModalAddAnswer(question.id)"
                @refresh="fetchData"/>
              </div>
            </li>
          </ul>
      </div>
      </div>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { ref, onMounted } from 'vue';
  import { useBonusQuestionsStore, usePredictionGameStore } from '@/stores/stores';
  import type { BonusQuestion } from '~/types/bonusQuestion';
  import type { BonusQuestionAnswer } from '~/types/bonusQuestionAnswer';

  const isGameCreator = ref(false);
  const userStore = useUserStore();
  const predictionGameStore = usePredictionGameStore();
  const bonusQuestionStore = useBonusQuestionsStore();
  const userId = userStore.user?.id;
  const predictionGameCreatorId = ref<number | null>(null);
  const showModal = ref(false);
  const showModalAddAnswer = ref<{ [key: number]: boolean }>({});;
  const hasAnsweredMap = ref<{ [key: number]: boolean }>({});
  
  const closeModal = () => {
  showModal.value = false; 
  };

  const closeModalAddAnswer = (questionId: number) => {
  showModalAddAnswer.value[questionId] = false; 
  };
  
  
  const props = defineProps<{
    predictionGameId: number;
  }>();
  
  const bonusQuestions = ref<BonusQuestion[]>([]);

  async function fetchData() {
    const gameId = props.predictionGameId;
    bonusQuestions.value = await bonusQuestionStore.getPredictionGameBonusQuestions(gameId);

    const predictionGame = await predictionGameStore.getPredictionGameById(props.predictionGameId);
    if (predictionGame) {
        predictionGameCreatorId.value = predictionGame.gameCreatorId;
        isGameCreator.value = userId === predictionGameCreatorId.value;
    }

    if (userId) {
    const answersMap: { [key: number]: BonusQuestionAnswer | null } = {};
    await Promise.all(bonusQuestions.value.map(async (question) => {
      await bonusQuestionStore.loadUserAnswer(question.id);
      answersMap[question.id] = bonusQuestionStore.userAnswer ? 
        { ...(bonusQuestionStore.userAnswer as BonusQuestionAnswer) } : null;
    }));
    for (const question of bonusQuestions.value) {
      const hasMadeAnswer = await userHasMadeAnswer(question, userId);
      hasAnsweredMap.value[question.id] = hasMadeAnswer;
    }
  }
  }

  
  onMounted(async () => {
    fetchData()
    
  });

  

  async function userHasMadeAnswer(question: BonusQuestion, userId: number): Promise<boolean> {
  await bonusQuestionStore.loadAnswers(question.id);
  return bonusQuestionStore.answers.some(element => element.answerMakerId === userId);
}

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

.btn-secondary {
  padding: 6px 18px;
  font-size: 14px;
  font-weight: bold;
  color: #fff;
  background-color: #5bb17c;
  border-radius: 5px;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.3s;
}

.btn-secondary:hover {
  background-color: #26547C;
  transform: scale(1.05);
}

.btn-secondary:active {
  background-color: #26547C;
  transform: scale(1);
}

</style>
  