<template>
  <div class="main-page h-screen bg-gray-100 p-8">
    <div class="header-section text-center mb-8">
      <h1 class="title-text">Welcome, {{ userName }}!</h1>
      <p class="subtitle-text">Your hub for football predictions and competitions.</p>
    </div>

    <!-- Section for New Users with No Games -->
    <div v-if="!hasPredictionGames" class="empty-state text-center mt-12">
      <h2 class="empty-title">Get Started with Prediction Games!</h2>
      <p class="empty-description">You haven't joined or created any games yet. Create or join a game to start competing with friends.</p>
      <div class="button-group flex justify-center space-x-4 mt-4">
        <button class="btn-primary" @click="ToCreateGame">Create Game</button>
        <button class="btn-secondary" @click="ToJoinGame">Join Game</button>
      </div>
    </div>

    <!-- User's Content Section -->
    <div v-else class="content-sections grid grid-cols-1 md:grid-cols-2 gap-8">
      <!-- Prediction History -->
      <div class="section-box bg-white p-6 rounded shadow">
        <h2 class="section-title">Your Prediction History</h2>
        <div v-if="formattedPredictionGamesHistory.length === 0" class="mt-6">
          <p class="empty-text text-center text-gray-500">No past predictions yet.</p>
        </div>
        <div v-else class="mt-8">
          <UTable :rows="formattedPredictionGamesHistory" :columns="columns">
            <template #teamNames-data="{ row }">
              <div v-html="row.teamNames"></div>
            </template>
            <template #actions-data="{ row }">
              <div class="flex items-center space-x-4">
              </div>
            </template>
          </UTable>
        </div>
      </div>

      <!-- Upcoming Predictions -->
      <div class="section-box bg-white p-6 rounded shadow">
        <h2 class="section-title">Upcoming Predictions</h2>
        <div v-if="upcomingPredictions.length === 0" class="mt-6">
          <p class="empty-text text-center text-gray-500">No upcoming predictions at the moment.</p>
        </div>
        <div v-else class="mt-8">
          <UTable :rows="formattedUpcomingPredictions" :columns="upcomingColumns">
            <template #teamNames-data="{ row }">
              <div v-html="row.teamNames"></div>
            </template>
            <template #actions-data="{ row }">
              <button @click="openPredictionModal(row)" class="btn-primary-small">
                Make a prediction
              </button>
            </template>
          </UTable>
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
      <div class="create-game-section bg-white p-6 rounded shadow text-center">
        <h2 class="section-title">Create a New Prediction Game</h2>
        <p class="description mt-2">Start a new competition with friends and test your prediction skills!</p>
        <button class="btn-primary mt-4" @click="ToCreateGame">Create Game</button>
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

const userStore = useUserStore();
const predictionGameStore = usePredictionGameStore();
const gameEventStore = useGameEventsStore();
const predictionsStore = usePredictionsStore();
const router = useRouter();

const showPredictionModal = ref(false);
const selectedGameEventId = ref(0);
const selectedPredictionGameId = ref(0);

const userName = computed(() => userStore.user?.userName || 'User');
const hasPredictionGames = computed(() => predictionGameStore.predictionGames.length > 0);
const predictionHistory = computed(() => predictionsStore.userPredictionHistory || []);
const upcomingPredictions = computed(() => gameEventStore.userUpcomingPredictions || []);
//const leaderboards = computed(() => userStore.user?.leaderboards || []);

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
  return predictionHistoryWithScores.value.map((prediction) => {
    return {
      teamNames: prediction.teamNames ? `${prediction.teamNames[0]} <br> ${prediction.teamNames[1]}` : "Error loading teams",
      yourPrediction: `${prediction.endScoreTeamA} - ${prediction.endScoreTeamB}`,
      score: prediction.score,
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
.main-page {
  max-width: 1200px;
  margin: 0 auto;
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
  background-color: #4CAF50;
  color: #fff;
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-secondary {
  background-color: #3498db;
  color: #fff;
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-link {
  color: #3498db;
  background: none;
  border: none;
  cursor: pointer;
  text-decoration: underline;
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
</style>
