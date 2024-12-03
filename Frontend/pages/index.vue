<template>
    <div class="main-page">
      
      <AppHeaderTransparent :scrollTarget="mainContentRef" v-if="isHomePage" />

      <!-- Top Section -->
      <div ref="mainContentRef" class="grid grid-cols-1 sm:grid-cols-2 gap-8 mt-12 p-8">
        <!-- Left Column -->
        <div class="sm:col-span-1">
          <h1 class="text-4xl font-bold font-merriweather-900">WELCOME TO YOUR PREDICTION GAMES HUB</h1>
          <p class="text-gray-600 mt-2 font-merriweather-400">
            See your upcoming predictions, previous activity and more!
          </p>
          <div class="mt-6 flex space-x-6">
            <button class="btn-primary font-bold font-weight-900" @click="openAddPredictionGame">CREATE YOUR OWN PREDICTION GAME</button>
            <button class="btn-primary font-bold font-weight-900" @click="ToJoinGame">JOIN A PREDICTION GAME</button>
          </div>
            <div 
              v-if="showAddPredictionGame" 
              class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50 overflow-auto modal-overlay">
              <AddPredictionGame @close="closeAddPredictionGame" />
            </div>
        </div>

        <!-- Right Column -->
        <div class="section-box bg-white p-2 rounded shadow sm:col-span-1 sm:w-2/3 ml-20">
          <h2 class="text-lg font-bold font-merriweather-400 text-center">Who wins tonight?</h2>

          <!-- Poll Options -->
          <div class="poll-rectangle relative flex mt-2 w-full rounded-lg border border-gray-300">
            <!-- Team A Background -->
            <div
              class="team-bg team-a-bg absolute h-full"
              :style="{ 
                width: showResults ? `${teamAPercentage}%` : '50%',
                backgroundColor: selectedOption === 'Team A' ? '#26547C' : '#26547C8b' }">
            </div>

            <!-- Team B Background -->
            <div
              class="team-bg team-b-bg absolute h-full"
              :style="{ 
                width: showResults ? `${teamBPercentage}%` : '50%',
                backgroundColor: selectedOption === 'Team B' ? '#26547C' : '#26547C8b' }">
            </div>

            <!-- Divider -->
            <div
              class="divider absolute h-full"
              :style="{ left: showResults ? `${teamAPercentage}%` : '50%', transition: 'left 0.5s ease' }">
            </div>

            <!-- Team A Section -->
            <div
              class="team-section flex relative items-center text-center cursor-pointer w-1/2"
              :class="{ selected: selectedOption === 'Team A', 'static-result': showResults }"
              @click="vote('Team A')">
              <span class="team-name font-merriweather-400 absolute inset-0 flex items-center justify-center">
                {{ homeTeamName }}
              </span>
              <div
                v-if="showResults"
                class="percentage-overlay absolute inset-0 flex items-end justify-center">
                <span><strong>{{ `${teamAPercentage}%` }}</strong></span>
              </div>
            </div>

            <!-- Team B Section -->
            <div
              class="team-section flex-1 relative text-center cursor-pointer"
              :class="{ selected: selectedOption === 'Team B', 'static-result': showResults }"
              @click="vote('Team B')">
              <span class="team-name font-merriweather-400 absolute inset-0 flex items-center justify-center">
                {{ awayTeamName }}
              </span>
              <div
                v-if="showResults"
                class="percentage-overlay absolute inset-0 flex items-end justify-center">
                <span><strong>{{ `${teamBPercentage}%` }}</strong></span>
              </div>
            </div>
          </div>
          <div v-if="showResults" class="text-center mt-4">
            <p class="text-sm text-gray-600 font-merriweather-400">
              Based on {{ totalVotes }} votes
            </p>
          </div>
        </div>
      </div>  

      <!-- Leaderboards -->
      <!-- <div class="section-box bg-white p-6 rounded shadow">
        <h2 class="section-title">Your Leaderboards</h2>
        <ul v-if="leaderboards.length" class="leaderboard-list mt-4">
          <li v-for="(board, index) in leaderboards" :key="index" class="leaderboard-item">
            <span>{{ board.gameName }}</span> - <strong>{{ board.rank }}</strong>
          </li>
        </ul>
        <p v-else class="empty-text">You're not currently ranked in any games.</p>
      </div> -->

      <!-- Bottom Section -->
      <div class="grid grid-cols-1 sm:grid-cols-2 gap-16 mt-12 p-8">
        <!-- Left Table: Upcoming Predictions -->
        <div class="section-box bg-white p-6 rounded shadow">
          <h2 class="text-lg font-bold font-merriweather-400">Upcoming Events That Await Your Predictions</h2>
          <div v-if="upcomingPredictions.length === 0">
            <p class="text-gray-500">No upcoming predictions at the moment.</p>
          </div>
          <div v-else>
            <UTable :rows="paginatedUpcomingPredictions" :columns="upcomingColumns" class="styled-table">
              <template #teamNames-data="{ row }">
                <div v-html="row.teamNames"></div>
              </template>
              <template #actions-data="{ row }">
                <button @click="openPredictionModal(row)" class="btn-primary-small">Make a prediction</button>
              </template>
            </UTable>
            <div class="flex justify-end px-3 py-3.5 border-t border-gray-200 dark:border-gray-700">
              <UPagination v-model="upcomingPage" :page-count="upcomingPageSize" :total="formattedUpcomingPredictions.length" class="bg-primary-500"/>
            </div>
          </div>
        </div>

        <div v-if="showPredictionModal" class="modal-overlay" @click="closePredictionModal">
          <div class="modal-content" @click.stop>
            <AddPrediction
              :gameEventId="selectedGameEventId"
              :predictionGameId="selectedPredictionGameId"
              
              @close="closePredictionModal" />
          </div>
        </div>

        <!-- Right Table: Prediction History -->
        <div class="section-box bg-white p-6 rounded shadow">
          <h2 class="text-lg font-bold font-merriweather-400">Your Predictions History and Points Earned</h2>
          <div v-if="formattedPredictionGamesHistory.length === 0">
            <p class="text-gray-500">No past predictions yet.</p>
          </div>
          <div v-else>
            <UTable :rows="formattedPredictionGamesHistory" :columns="columns" class="styled-table">
              <template #teamNames-data="{ row }">
                <div v-html="row.teamNames"></div>
              </template>
            </UTable>
          </div>
        </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useUserStore, type GameEvent } from '#imports';
import { format } from 'date-fns';
import { errorMessages } from 'vue/compiler-sfc';
import { onBeforeRouteLeave } from 'vue-router';
import AppHeaderTransparent from '~/components/AppHeaderTransparent.vue';
import { _width } from '#tailwind-config/theme';
import { useDailyPollStore } from '~/stores/dailyPollStore';

const route = useRoute();
const isHomePage = computed(() => route.path === '/');
const mainContentRef = ref(null);

const userStore = useUserStore();
const predictionGameStore = usePredictionGameStore();
const gameEventStore = useGameEventsStore();
const predictionsStore = usePredictionsStore();
const dailyPollStore = useDailyPollStore();
const router = useRouter();

const showPredictionModal = ref(false);
const selectedGameEventId = ref(0);
const selectedPredictionGameId = ref(0);
const selectedOption = ref("");
const showResults = ref(false);

const homeTeamName = ref<string | null>(null);
const awayTeamName = ref<string | null>(null);

const votes = ref({ teamA: 0, teamB: 0 });
const totalVotes = computed(() => votes.value.teamA + votes.value.teamB);

const vote = (team: "Team A" | "Team B") => {
  // if (showResults.value) return; tulevikus maha votta, kui iga kasutaja ainult uhe ennustuse teeb
  selectedOption.value = team;
  showResults.value = true;

  if (team === "Team A") votes.value.teamA++;
  else votes.value.teamB++;
};

const teamAPercentage = computed(() => totalVotes.value === 0 ? 50 : Math.round((votes.value.teamA / totalVotes.value) * 100));
const teamBPercentage = computed(() => {
  if (totalVotes.value === 0) return 50;
  const percentageB = (votes.value.teamB / totalVotes.value) * 100;
  
  if (percentageB + teamAPercentage.value !== 100) {
    const adjustedB = 100 - teamAPercentage.value;
    return Math.round(adjustedB);
  }
  return percentageB;
});

const userName = computed(() => userStore.user?.userName || 'User');
const hasPredictionGames = computed(() => predictionGameStore.predictionGames.length > 0);
const predictionHistory = computed(() => predictionsStore.userPredictionHistory || []);
const upcomingPredictions = computed(() => gameEventStore.userUpcomingPredictions || []);
//const leaderboards = computed(() => userStore.user?.leaderboards || []);

const upcomingPage = ref(1);
const upcomingPageSize = 4;
const paginatedUpcomingPredictions = computed(() => {
  const start = (upcomingPage.value - 1) * upcomingPageSize;
  return formattedUpcomingPredictions.value.slice(start, start + upcomingPageSize);
});

type PredictionWithScoreTeamnamesPoints = {
  id: number;
  endScoreTeamA: number;
  endScoreTeamB: number;
  predictionMakerId: number;
  eventId: number;
  score?: string;
  teamNames? : [string, string];
  points?: number;
};

const predictionHistoryWithScores = ref<PredictionWithScoreTeamnamesPoints[]>([]);

const formattedPredictionGamesHistory = computed(() => {
  const latestFivePredictions = predictionHistoryWithScores.value
    .slice(-5)
    .reverse();

  return latestFivePredictions.map((prediction) => {
    return {
      teamNames: prediction.teamNames
        ? `${prediction.teamNames[0]} <br> ${prediction.teamNames[1]}`
        : "Error loading teams",
      yourPrediction: `${prediction.endScoreTeamA} - ${prediction.endScoreTeamB}`,
      score: prediction.score || "Not available",
      points: prediction.points,
    };
  });
});

const formattedUpcomingPredictions = computed(() => {
  return upcomingPredictions.value.map((event : GameEvent) => {
    return {
      ...event,
      eventDate: event.eventDate? format(new Date(event.eventDate), 'dd.MM.yyyy HH:mm') : '',
      teamNames: `${event.teamA} <br> ${event.teamB}`
    }
  })
})

const loadAllUserUpcomingPredictions = async () => {
  const predictionGameIds = predictionGameStore.predictionGames.map(game => game.id);

  for (const gameId of predictionGameIds) {
    await gameEventStore.loadUserUpcomingPredictions(gameId)
  }
};

onMounted(async () => {
  await predictionGameStore.loadPredictionGames();
  await predictionsStore.loadUserPredictionHistory();
  await loadAllUserUpcomingPredictions();

  predictionHistoryWithScores.value = await Promise.all(
    predictionHistory.value.map(async (prediction) => {
      const score = await getScore(prediction.eventId);
      const teamNames: [string, string] = await getTeamNames(prediction.eventId);
      const points: number = await getEventPoints(prediction.eventId);
      return { ...prediction, score, teamNames, points };
    })
  );

  await dailyPollStore.getDailyPollTeams();
  homeTeamName.value = dailyPollStore.homeTeamName;
  awayTeamName.value = dailyPollStore.awayTeamName;
});

onBeforeRouteLeave(() => {
  gameEventStore.userUpcomingPredictions = [];
});

const getScore = async (eventId: number): Promise<string> => {
  try {
    const event = await gameEventStore.loadSingleEvent(eventId) as GameEvent;
    if (event && event.teamAScore !== null && event.teamBScore !== null) {
      const score = `${event.teamAScore} - ${event.teamBScore}`
      return score;
    }
    return "final score not inserted";
  } catch (error) {
    return "Error loading score";
  }
};

const getTeamNames = async (eventId: number): Promise<[string, string]> => {
  try {
    const event = await gameEventStore.loadSingleEvent(eventId) as GameEvent;
    if (event && event.teamA && event.teamB) {
      const teamNames : [string, string] = [event.teamA, event.teamB]
      return teamNames;
    }
    return ["Name missing", "Name missing"];
  } catch (error) {
    return ["Error loading team name", "Error loading team name"];
  }
}

const getEventPoints = async (eventId: number): Promise<number> => {
  try {
    const event = await gameEventStore.loadSingleEvent(eventId) as GameEvent;
    if (event) {
      const predictionGame = await predictionGameStore.getPredictionGameById(event.predictionGameId);
      if (predictionGame) {
        const points: number = await gameEventStore.loadUserPointsForEvent(predictionGame.id, event.id);
        return points;
      }
    }
    return 0;
  } catch (error) {
    return 0;
  }
}

const columns = [
  { key: "teamNames", label: "Teams" },
  { key: "yourPrediction", label: "Your Prediction" },
  { key: "score", label: "Actual Score" },
  { key: "points", label: "Points"},
  { key: "actions", label: "" }
];

const upcomingColumns = [
  { key: "eventDate", label: 'Event Date' },
  { key: "teamNames", label: "Teams" },
  { key: "actions", label: '' },
];

const showAddPredictionGame = ref(false);
function openAddPredictionGame() {
  showAddPredictionGame.value = true;
}
function closeAddPredictionGame() {
  showAddPredictionGame.value = false;
}

const ToJoinGame = () => {
  router.push('/join-game');
};

function openPredictionModal(row : GameEvent) {
  selectedGameEventId.value = row.id;
  selectedPredictionGameId.value = row.predictionGameId;
  showPredictionModal.value = true;
}

function closePredictionModal() {
  showPredictionModal.value = false;
  gameEventStore.userUpcomingPredictions = [];
  loadAllUserUpcomingPredictions();
}
</script>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Merriweather:wght@400;700;900&display=swap');

.font-merriweather-900 {
  font-family: 'Merriweather', sans-serif;
  font-weight: 900;
}
.font-merriweather-400 {
  font-family: 'Merriweather', sans-serif;
  font-weight: 400;
}

.main-page {
  width: 100%;
  margin: 0 auto;
  min-height: 100vh;
  display: flex;
  flex-direction: column;
  padding: 0;
}

.bg-primary-500 {
  --tw-bg-opacity: 1;
  background-color: rgb(91, 177, 124 / var(--tw-bg-opacity)) !important;
}

.up-pagination__item {
  color: #090f0b;
}

.pagination .up-pagination__item:hover {
  color: #5bb17c;
}

.section-box {
  background-color: #f0fff166;
  border-radius: 30px;
  padding: 1rem;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  
}

.poll-rectangle {
  position: relative;
  background-color: #f9fafb;
  height: 70px;
  overflow: hidden;
  transition: background-color 0.3s, width 0.3s;
  border-radius: 8px;
}

.team-section {
  position: relative;
  padding: 16px 0;
  font-size: 16px;
  font-weight: bold;
  background-color: #26547C8b;
  transition: flex-grow 0.3s, background-color 0.3s;
  transition: width 0.5s ease;
}

.team-section:hover {
  background-color: #B8B3E9;
}

.team-section.selected {
  background-color: #26547C8b;
  color: white;
}

.team-bg {
  position: absolute;
  top: 0;
  bottom: 0;
  z-index: 1;
  transition: width 0.5s ease;
}

.team-a-bg {
  left: 0;
}

.team-b-bg {
  right: 0;
}

.divider {
  position: absolute;
  z-index: 1;
  width: 3.5px;
  background-color: #26547C;
}

.static-result {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #26547C8b;
}

.team-name {
  position: absolute;
  z-index: 1;
  color: #f9fafb;
  font-size: 20px;
  font-weight: bold;
}

.percentage-overlay {
  position: absolute;
  z-index: 2;
  color: white;
  font-size: 14px;
  font-weight: bold;
}

.title-text {
  font-size: 2.5rem;
  font-weight: 700;
  color: #2e3747;
}

.subtitle-text {
  font-size: 1.25rem;
  color: #5a6677;
}

.section-box {
  border: 1px solid #ddd;
}

.section-title {
  font-size: 1.5rem;
  font-weight: 600;
  color: #34495e;
}

.btn-primary {
  background-color: #5bb17c;
  color: #fff;
  padding: 10px 28px;
  border: none;
  border-radius: 80px;
  cursor: pointer;
  font-family: 'Merriweather', sans-serif;
  transition: background-color 0.3s, transform 0.3s;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.btn-primary:hover {
  background-color: #26547C;
  transform: scale(1.05)
}

.btn-primary:active {
  background-color: #26547C;
  transform: scale(1);
}

.btn-primary-small {
  padding: 5px 15px;
  font-size: 14px;
  font-weight: normal;
  color: #ffffff;
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

.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
}
.modal-content {
  background: white;
  border-radius: 4px;
  width: 90%;
  max-width: 600px;
  max-height: 80%;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  position: relative;
  z-index: 100;
  overflow-y: auto;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}

@keyframes slideIn {
  from {
    transform: translateY(-20px);
  }
  to {
    transform: translateY(0);
  }
}

.styled-table {
    width: 100%;
    border-collapse: collapse;
    overflow-x: auto;
}
</style>
