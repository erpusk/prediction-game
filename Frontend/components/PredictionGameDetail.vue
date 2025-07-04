  <template>
    <div class="min-h-screen bg-gray-100 flex justify-center items-start  dark:bg-gray-900">
      <div class="detail-page">
        <h2 class="text-4xl font-semibold text-center mb-8 text-gray-800 dark:text-white">{{ game.title }} details</h2>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Game Creator:</strong> {{
              game.gameCreator }}</p>
          </div>

          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500 ">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Created at:</strong> {{
              game.creationDate }}</p>
          </div>

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Start date:</strong> {{
              game.startDate }}</p>
          </div>

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>End date:</strong> {{
              game.endDate }}</p>
          </div>

          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white" style="white-space: pre-line;">
              <strong>Game privacy:</strong> {{ game.privacy }}
            </p>
          </div>

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
          <p class="text-lg font-medium text-gray-700 text-center dark:text-white" >
            <strong>Joined players:</strong>
          </p>
          <div class="flex flex-col items-center">
            
            <div v-for="(participant, index) in game.participants" :key="index" class="flex items-center space-x-3">
              <button @click="openModal(participant)">
                <span class="text-lg font-medium text-gray-700 dark:text-white">{{ participant[0] }}</span>
              </button>

              <div 
                v-if="showModal && selectedParticipant" 
                class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-10 z-50">
               <ParticipantDetails :id="selectedParticipant[2]" @close="closeModal" />
              </div>

              <img 
              v-if="participant[1] != ''"
              :src="decodeProfilePicture(participant[1])" 
              alt="Profile Picture" 
              class="w-7 h-7 rounded-full object-cover">
              <div v-else class="w-7 h-7 rounded-full bg-gray-400 flex items-center justify-center text-white text-lg">
                <span>{{ participant[0][0] }}</span>
              </div>
            </div>
          </div>
          </div>

          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white" style="white-space: pre-line;">
              <strong>Your points:</strong> {{ game.userEarnedPoints }}
            </p>
          </div>
          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
            <h3 class="text-lg font-medium text-gray-700 text-center mb-4 dark:text-white">
              <strong>Top 5 players:</strong>
            </h3>
            <ul class="space-y-2">
              <li v-for="(entry, index) in game.leaderBoard.splice(0, 5)" :key="index"
                class="flex justify-between items-center p-2 bg-gray-100 rounded-lg dark:bg-gray-600">

                <div class="flex items-center">
                  <span class="mr-2">
                    <span v-if="index === 0">🥇</span>
                    <span v-else-if="index === 1">🥈</span>
                    <span v-else-if="index === 2">🥉</span>
                    <span v-else class="font-semibold mr-1  " style="margin-left: 7px;">{{ index + 1 }}.</span>
                  </span> 
                  <span class="font-semibold dark:text-white">{{ entry.username }}</span>
                </div>
                <span class="font-semibold dark:text-white">{{ entry.points }} points</span>
              </li>
            </ul>
          </div>
        </div>
      </div>
    </div>
  </template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute } from 'vue-router';
import { usePredictionGameStore } from '@/stores/stores';
import { parse, format } from 'date-fns';
import type { BonusQuestion } from '~/types/bonusQuestion';

  const props = defineProps<{
    predictionGameId: number;
  }>();
  const bonusQuestionsStore = useBonusQuestionsStore();
  const predictionGameStore = usePredictionGameStore();
  const showModal = ref(false);
  const selectedParticipant = ref<[string, string, number] | null>(null);

  const closeModal = () => {
    showModal.value = false; 
    selectedParticipant.value = null;
  };

  const openModal = (participant: [string, string, number]) => {
    selectedParticipant.value = participant;
    showModal.value = true;
  };

  const game = ref({
    id: parseInt(props.predictionGameId.toString(), 10),
    startDate: '',
    endDate: '',
    creationDate: '',
    privacy: '',
    title: '',
    participants: [] as [string, string, number][],
    gameCreator: '',
    userEarnedPoints: '',
    leaderBoard: [] as { username: string, points: number }[],
    bonusQuestions: [] as BonusQuestion[]
  });


onMounted(async () => {
  const predictionGame = await predictionGameStore.loadPredictionGame(props.predictionGameId);
  game.value.startDate = predictionGame.startDate ? format(new Date(predictionGame.startDate), 'dd.MM.yyyy') : '';
  game.value.endDate = predictionGame.endDate ? format(new Date(predictionGame.endDate), 'dd.MM.yyyy') : '';
  game.value.creationDate = predictionGame.creationDate ? format(new Date(predictionGame.creationDate), 'dd.MM.yyyy') : '';
  game.value.privacy = predictionGame.privacy;
  game.value.title = predictionGame.predictionGameTitle

    const participants: [string, string, number][] = [];
    predictionGame.participants.forEach(async element => {
      participants.push([element.userName, element.profilePicture, element.userId])
      if (element.userId === predictionGame.gameCreatorId){
        game.value.gameCreator = element.userName
      }
    });
    
    game.value.participants = participants

  const userPoints = await predictionGameStore.loadUserPoints(props.predictionGameId);
  game.value.userEarnedPoints = userPoints !== null ? `${userPoints} points` : 'No points available';

  const leaderBoard = await predictionGameStore.getLeaderboard(props.predictionGameId);
  console.log("API Response:", leaderBoard);

  if (leaderBoard) {
    game.value.leaderBoard = leaderBoard.map(item => ({
      username: item.username,
      points: item.points,
    }));
    console.log("Updated leaderBoard in game:", game.value.leaderBoard);
  }
  
  game.value.bonusQuestions = await bonusQuestionsStore.getPredictionGameBonusQuestions(props.predictionGameId)
});

function decodeProfilePicture(picString: any){
  return `data:image/jpeg;base64,${picString}`;
}

</script>

<style scoped>
.detail-page {
  padding: 20px;
  background-color: white;
  border-radius: 8px;
  max-width: 600px;
  margin: 20px auto;
}

.dark .detail-page {
  background-color: #1f2937;
}
</style>
