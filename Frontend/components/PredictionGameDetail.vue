  <template>
    <div class="min-h-screen bg-gray-100 flex justify-center items-start  dark:bg-gray-900">
      <div class="detail-page">
        <h2 class="text-4xl font-semibold text-center mb-8 text-gray-800 dark:text-white">{{ game.title }} details</h2>

        <div class="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Privacy:</strong> {{
              game.privacy }}</p>
          </div>

          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500 ">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Start date:</strong> {{
              game.startDate }}</p>
          </div>

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>End date:</strong> {{
              game.endDate }}</p>
          </div>

          <div class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white"><strong>Creation date:</strong> {{
              game.creationDate }}</p>
          </div>

          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white" style="white-space: pre-line;">
              <strong>Game creator:</strong> {{ game.gameCreator }}
            </p>
          </div>

          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white" style="white-space: pre-line;">
              <strong>Joined players:</strong> {{ game.participants }}
            </p>
          </div>

          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
            <p class="text-lg font-medium text-gray-700 text-center dark:text-white" style="white-space: pre-line;">
              <strong>Your points:</strong> {{ game.userEarnedPoints }}
            </p>
          </div>
          <div
            class="border border-gray-200 rounded-lg p-4 bg-gray-50 shadow-sm md:col-span-2 dark:bg-gray-700 dark:border-gray-500">
            <h3 class="text-lg font-medium text-gray-700 text-center mb-4 dark:text-white"><strong>Leaderboard:</strong>
            </h3>
            <ul class="space-y-2">
              <li v-if="game.leaderBoard.length > 0" 
                v-for="(entry, index) in game.leaderBoard" :key="index"
                class="flex justify-between items-center p-2 bg-gray-100 rounded-lg dark:bg-gray-600">
                <span class="font-semibold dark:text-white">{{ index + 1 }}. {{ entry.username }}</span>
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
import { parse } from 'date-fns';

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
  participants: '',
  gameCreator: '',
  userEarnedPoints: '',
  leaderBoard: [] as { username: string, points: number }[],
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
    participantsUsernames.push(element.userName)
    if (element.userId === predictionGame.gameCreatorId) {
      game.value.gameCreator = element.userName
    }
  });

  game.value.participants = "\n" + participantsUsernames.join('\n');

  const userPoints = await predictionGameStore.loadUserPoints(parseInt(props.id.toString()));
  game.value.userEarnedPoints = userPoints !== null ? `${userPoints} points` : 'No points available';

  const leaderBoard = await predictionGameStore.getLeaderboard(parseInt(props.id.toString()));
  console.log("API Response:", leaderBoard);

  if (leaderBoard) {
    game.value.leaderBoard = leaderBoard.map(item => ({
      username: item.username,
      points: item.points,
    }));
    console.log("Updated leaderBoard in game:", game.value.leaderBoard);
  }
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

.dark .detail-page {
  background-color: #1f2937;
}
</style>
