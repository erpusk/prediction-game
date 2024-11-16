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
      <!-- <div class="section-box bg-white p-6 rounded shadow">
        <h2 class="section-title">Your Prediction History</h2>
        <UTable v-if="predictionHistoryWithScores.length" class="prediction-table mt-4">
          <thead>
            <tr>
              <th>Teams</th>
              <th>Your Prediction</th>
              <th>Actual Score</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(prediction, index) in predictionHistoryWithScores" :key="index">
              <td>
                <div>
                  Team A: {{ prediction.teamNames[0] }}<br />
                  Team B: {{ prediction.teamNames[1] }}
                </div>
              </td>
              <td>{{ prediction.endScoreTeamA }} - {{ prediction.endScoreTeamB }}</td>
              <td>{{ prediction.score }} </td>
            </tr>
          </tbody>
        </UTable>
        <p v-else class="empty-text">No past predictions yet.</p>
      </div> -->
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

      <!-- To-Do Predictions -->
      <div class="section-box bg-white p-6 rounded shadow">
        <h2 class="section-title">Upcoming Predictions</h2>
        <ul v-if="upcomingPredictions.length" class="todo-list mt-4">
          <li v-for="(prediction, index) in upcomingPredictions" :key="index" class="todo-item">
            <span>{{ prediction.eventId }}</span> - <button class="btn-link" @click="ToJoinGame">Make Prediction</button>
          </li>
        </ul>
        <p v-else class="empty-text">No upcoming predictions at the moment.</p>
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

const userStore = useUserStore();
const predictionGameStore = usePredictionGameStore();
const gameEventStore = useGameEventsStore();
const predictionsStore = usePredictionsStore();
const router = useRouter();

const userName = computed(() => userStore.user?.userName || 'User');
const hasPredictionGames = computed(() => predictionGameStore.predictionGames.length > 0);
const predictionHistory = computed(() => predictionsStore.userPredictionHistory || []);
//const predictionHistoryWithScores = ref([]);
const upcomingPredictions = computed(() => predictionsStore.userUpcomingPredictions || []);
//const leaderboards = computed(() => userStore.user?.leaderboards || []);

type PredictionWithScore = {
  id: number;
  endScoreTeamA: number;
  endScoreTeamB: number;
  predictionMakerId: number;
  eventId: number;
  score: string;
  teamNames : [string, string];
};

const predictionHistoryWithScores = ref<PredictionWithScore[]>([]);

const formattedPredictionGamesHistory = computed(() => {
  return predictionHistoryWithScores.value.map((prediction) => {
    return {
      teamNames: `${prediction.teamNames[0]} <br> ${prediction.teamNames[1]}`,
      yourPrediction: `${prediction.endScoreTeamA} - ${prediction.endScoreTeamB}`,
      score: prediction.score,
    };
  });
});

onMounted(async () => {
  await predictionGameStore.loadPredictionGames();
  await predictionsStore.loadUserPredictionHistory();
  predictionsStore.loadUserUpcomingPredictions();

  predictionHistoryWithScores.value = await Promise.all(
    predictionHistory.value.map(async (prediction) => {
      const score = await getScore(prediction.eventId);
      const teamNames: [string, string] = await getTeamNames(prediction.eventId)
      return { ...prediction, score, teamNames };
    })
  );
  console.log(predictionHistoryWithScores.value);
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
    console.error("Error fetching event:", error);
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

const columns = [
  { key: "teamNames", label: "Teams" },
  { key: "yourPrediction", label: "Your Prediction" },
  { key: "score", label: "Actual Score" },
  { key: "points", label: "Points"},
  { key: "actions", label: "" }
];

const ToCreateGame = () => {
  router.push('/add-predictiongame');
};
const ToJoinGame = () => {
  router.push('/join-game');
};
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

.empty-state {
  color: #5a6677;
}
</style>
