<template>
    <div class="main-page">
      
      <AppHeaderTransparent v-if="isHomePage" />
    
    <!-- Background Section with Greeting -->
    <!-- <div class="hero-section relative h-screen bg-cover bg-center flex items-center justify-center text-white">
      <div class="overlay absolute inset-0 bg-black bg-opacity-50"></div>
      <div class="greeting-message relative text-center">
        <h1 class="text-6xl font-bold mb-6">Turn rivalries to memories!</h1>
        <p class="text-xl">Kick off your Prediction Game today.</p>
      </div>
    </div> -->

    <!-- Section for New Users with No Games -->
    <!-- <div v-if="!hasPredictionGames" class="empty-state text-center mt-12">
      <h2 class="empty-title">Get Started with Prediction Games!</h2>
      <p class="empty-description">You haven't joined or created any games yet. Create or join a game to start competing with friends.</p>
      <div class="button-group flex justify-center space-x-4 mt-4">
        <button class="btn-primary" @click="ToCreateGame">Create Game</button>
        <button class="btn-secondary" @click="ToJoinGame">Join Game</button>
      </div>
    </div> -->

      <!-- Top Section -->
      <div class="grid grid-cols-1 sm:grid-cols-2 gap-8 mt-12 p-8">
        <!-- Left Column -->
        <div class="sm:col-span-1">
          <h1 class="text-4xl font-bold font-merriweather-900">WELCOME TO YOUR PREDICTION GAMES HUB</h1>
          <p class="text-gray-600 mt-2 font-merriweather-400">
            See your upcoming predictions, previous activity and more!
          </p>
          <div class="mt-6 flex space-x-6">
            <button class="btn-primary font-bold font-weight-900" @click="ToCreateGame">CREATE YOUR OWN PREDICTION GAME</button>
            <button class="btn-primary font-bold font-weight-900" @click="ToJoinGame">JOIN A PREDICTION GAME</button>
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
                backgroundColor: selectedOption === 'Team A' ? '#B0B0B0' : '#E8E8E8' }">
            </div>

            <!-- Team B Background -->
            <div
              class="team-bg team-b-bg absolute h-full"
              :style="{ 
                width: showResults ? `${teamBPercentage}%` : '50%',
                backgroundColor: selectedOption === 'Team B' ? '#B0B0B0' : '#E8E8E8' }">
            </div>

            <!-- Divider -->
            <div
              class="divider absolute h-full bg-gray-500"
              :style="{ left: showResults ? `${teamAPercentage}%` : '50%', transition: 'left 0.5s ease' }">
            </div>

            <!-- Team A Section -->
            <div
              class="team-section flex relative items-center text-center cursor-pointer w-1/2"
              :class="{ selected: selectedOption === 'Team A', 'static-result': showResults }"
              @click="vote('Team A')">
              <span class="team-name font-merriweather-400 absolute inset-0 flex items-center justify-center">
                Manchester City
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
                Liverpool FC
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

      <!-- Prediction History -->
      <!-- <div class="section-box bg-white p-6 rounded shadow">
        <h2 class="section-title">Your Prediction History</h2>
        <div v-if="formattedPredictionGamesHistory.length === 0" class="mt-6">
          <p class="empty-text text-center text-gray-500">No past predictions yet.</p>
        </div>
        <div v-else class="mt-1">
          <UTable :rows="formattedPredictionGamesHistory" :columns="columns" class="styled-table">
            <template #teamNames-data="{ row }">
              <div v-html="row.teamNames"></div>
            </template>
            <template #actions-data="{ row }">
              <div class="flex items-center space-x-4">
              </div>
            </template>
          </UTable>
        </div>
      </div> -->

      <!-- Upcoming Predictions -->
      <!-- <div class="section-box bg-white p-6 rounded shadow">
        <h2 class="section-title">Upcoming Predictions</h2>
        <div v-if="upcomingPredictions.length === 0" class="mt-6">
          <p class="empty-text text-center text-gray-500">No upcoming predictions at the moment.</p>
        </div>
        <div v-else class="mt-1">
          <UTable :rows="paginatedUpcomingPredictions" :columns="upcomingColumns">
            <template #teamNames-data="{ row }">
              <div v-html="row.teamNames"></div>
            </template>
            <template #actions-data="{ row }">
              <button @click="openPredictionModal(row)" class="btn-primary-small">
                Make a prediction
              </button>
            </template>
          </UTable>
          <div class="flex justify-end px-3 py-3.5 border-t border-gray-200 dark:border-gray-700">
            <UPagination v-model="upcomingPage" :page-count="upcomingPageSize" :total="formattedUpcomingPredictions.length" />
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
      </div> -->

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

      <!-- Create a New Game Section -->
      <!-- <div class="create-game-section bg-white p-6 rounded shadow text-center">
        <h2 class="section-title">Create a New Prediction Game</h2>
        <p class="description mt-2">Start a new competition with friends and test your prediction skills!</p>
        <button class="btn-primary mt-4" @click="ToCreateGame">Create Game</button>
      </div> -->

      <!-- Bottom Section -->
      <div class="grid grid-cols-1 sm:grid-cols-2 gap-8 mt-12 p-8">
        <!-- Left Table: Upcoming Predictions -->
        <div class="section-box bg-white p-6 rounded shadow">
          <h2 class="text-lg font-bold">Upcoming Events That Await Your Predictions</h2>
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
            <div class="pagination flex justify-end px-3 py-3.5 border-t border-gray-200 dark:border-gray-700">
              <UPagination v-model="upcomingPage" :page-count="upcomingPageSize" :total="formattedUpcomingPredictions.length" />
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
          <h2 class="text-lg font-bold">Your Predictions History</h2>
          <div v-if="formattedPredictionGamesHistory.length === 0">
            <p class="text-gray-500">No past predictions yet.</p>
          </div>
          <div v-else>
            <UTable :rows="formattedPredictionGamesHistory" :columns="columns">
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

const route = useRoute();
const isHomePage = computed(() => route.path === '/');

const userStore = useUserStore();
const predictionGameStore = usePredictionGameStore();
const gameEventStore = useGameEventsStore();
const predictionsStore = usePredictionsStore();
const router = useRouter();

const showPredictionModal = ref(false);
const selectedGameEventId = ref(0);
const selectedPredictionGameId = ref(0);
const selectedOption = ref("");
const showResults = ref(false);
const votes = ref({ teamA: 0, teamB: 0 });
const totalVotes = computed(() => votes.value.teamA + votes.value.teamB);

const vote = (team: "Team A" | "Team B") => {
  // if (showResults.value) return; tulevikus maha votta, kui iga kasutaja ainult uhe ennustuse teeb
  selectedOption.value = team;
  showResults.value = true;

  if (team === "Team A") votes.value.teamA++;
  else votes.value.teamB++;
};

const teamAPercentage = computed(() => totalVotes.value === 0 ? 0 : Math.round((votes.value.teamA / totalVotes.value) * 100));
const teamBPercentage = computed(() => totalVotes.value === 0 ? 0 : Math.round((votes.value.teamB / totalVotes.value) * 100));

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

type PredictionWithScoreTeamnamesEventdate = {
  id: number;
  endScoreTeamA: number;
  endScoreTeamB: number;
  predictionMakerId: number;
  eventId: number;
  score?: string;
  teamNames? : [string, string];
};

const predictionHistoryWithScores = ref<PredictionWithScoreTeamnamesEventdate[]>([]);

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
      const teamNames: [string, string] = await getTeamNames(prediction.eventId)
      return { ...prediction, score, teamNames };
    })
  );
});

onBeforeRouteLeave(() => {
  gameEventStore.userUpcomingPredictions = [];
});

const getScore = async (eventId: number): Promise<string> => {
  try {
    const event = await gameEventStore.loadSingleEvent(eventId) as GameEvent;
    if (event && event.teamAScore && event.teamBScore) {
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

// const getEventDate = async (eventId: number): Promise<Date | string> => {
//   try {
//     const event = await gameEventStore.loadSingleEvent(eventId) as GameEvent;
//     if (event && event.eventDate) {
//       return event.eventDate;
//     }
//     return "Event date not found";
//   } catch (error) {
//     return "Error loading event date";
//   }
// }

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

const ToCreateGame = () => {
  router.push('/add-predictiongame');
};
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

.pagination .up-pagination__item {
  color: #090f0b;
}

.pagination .up-pagination__item:hover {
  color: #4c9266;
}

/* .hero-section {
  position: relative;
  background-image: url('/images/soccer-fans-cheering-team-monochrome (1).jpg');
  background-size: cover;
  background-position: center top;
  height: 100vh;
  width: 100%;
  color: white;
}
.hero-section .overlay {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.6);
  z-index: 1;
}

.greeting-message {
  position: relative;
  z-index: 2;
  text-align: center;
}

.greeting-message h1 {
  font-size: 3.5rem;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.8);
}

.greeting-message p {
  font-size: 2rem;
  text-shadow: 0 2px 4px rgba(0, 0, 0, 0.8);
}

.overlay {
  position: absolute;
  background: rgba(0, 0, 0, 0.5);
} */

.section-box {
  background-color: white;
  border-radius: 30px;
  padding: 1rem;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  
}

/* .poll-options button {
  padding: 0.5rem 1rem;
}

.poll-results {
  background-color: #f9fafb;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
}

.poll-results .bg-blue-500 {
  background-color: #3b82f6;
}

.poll-results .bg-red-500 {
  background-color: #f87171;
} */
.poll-rectangle {
  /* display: flex; */
  position: relative;
  background-color: #f9fafb;
  height: 80px;
  overflow: hidden;
  transition: background-color 0.3s, width 0.3s;
  border-radius: 8px;
}

.team-section {
  position: relative;
  padding: 16px 0;
  font-size: 16px;
  font-weight: bold;
  background-color: #e5e7eb;
  transition: flex-grow 0.3s, background-color 0.3s;
  transition: width 0.5s ease;
}

.team-section:hover {
  background-color: #d1d5db;
}

.team-section.selected {
  background-color: #575757;
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
  z-index: 5;
  width: 2px;
  background-color: #333;
}

.static-result {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #e5e7eb;
}

.team-name {
  position: absolute;
  z-index: 1;
  color: #1f2937;
  font-size: 20px;
  font-weight: bold;
}

.percentage-overlay {
  position: absolute;
  z-index: 2;
  color: white;
  font-size: 18px;
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
  background-color: #4d6bac;
  transform: scale(1.05)
}

.btn-primary:active {
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
  padding: 20px;
  border-radius: 4px;
  width: 90%;
  max-width: 600px;
  max-height: 80%;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  overflow-y: auto;
  position: relative;
  z-index: 100;
}

.empty-state {
  color: #5a6677;
}

.content-sections {
  display: grid;
  grid-template-columns: 1fr;
  grid-template-columns: repeat(2, 1fr);
  gap: 80px;
  flex-grow: 1;
  overflow: auto;
}

.styled-table {
    width: 100%;
    border-collapse: collapse;
    overflow: hidden;
}

.styled-table th, .styled-table td {
    text-align: left;
    padding: 120px;
}

.styled-table th {
    background-color: #5bb17c;
    color: white;
}

.styled-table td {
    border-bottom: 1px solid #ddd;
}

.footer {
  margin-top: auto;
  padding: 10px;
  background-color: #333;
  color: white;
  text-align: center;
}
</style>
