<template>
  <div class="min-h-screen bg-white flex justify-center items-center p-6">
  <div class="p-20 bg-white rounded-lg shadow-lg max-w-5x1">
    <button 
      v-if="isGameCreator"
      @click="showModal = true" 
      class="absolute top-20 right-10 bg-gradient-to-r from-blue-500 to-blue-700 
      hover:from-blue-600 hover:to-blue-800 text-white font-semibold py-3 px-6 rounded-md shadow-md 
      hover:shadow-lg transition duration-300 ease-in-out transform hover:scale-105">
      Create a new Event
    </button>
    <AddGameEvent 
      v-if="showModal" 
      :predictionGameId="parseInt(props.predictionGameId.toString())" 
      @close="closeModal" 
    />
    <div v-if="uniqueCode && uniqueCode !== 'Not available'"
     class="unique-code-box absolute top-32 right-10 mt-4 p-2 bg-gradient-to-r from-blue-500 to-blue-700 rounded text-center shadow-md">
     <div class="flex items-center justify-center"> 
     <p class="text-2xl font-bold mr-2">{{ uniqueCode }}</p>
        <button @click="copyToClipboard" class="copy-button flex items-center">
            <i class="fas fa-copy mr-1"></i>
          </button>
        </div>  
      </div>
      <div v-if="gameEvents.length === 0">
        <h2 class="text-x1 text-center">No events have been added</h2>
      </div>
    <div v-else class="mt-8">
      <h1 class="text-3xl font-bold text-center mb-6 text-black">{{ title }}</h1>
      <UTable :rows="formattedGameEvents" :columns="columns">

        <template #teams-data="{ row }">
          <div v-html="row.teams.replace('\n', '<br>')"></div> <!-- Replace newline with a line break -->
        </template>

        <template #actions-data="{ row }">
          <template v-if="isGameCreator">
              <button @click="deletePredictionGameEvent(row)" class="flex items-center text-red-500 hover:text-red-700">
                <DeleteIconComponent />
              </button>
              <button @click="goToEditPredictionGameEvent(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
                Edit
              </button>
          </template>
          <button @click="goToPredictionGameEventDetails(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
            Details
          </button>
          <div v-if="hasMadePredictionMap[row.id] === true">
            <button class="bg-gray-500 text-black rounded px-4 py-2">
              Cant make prediction
            </button>
          </div>
          <div v-else>
            <button @click="goToMakingAPrediction(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
                Make a prediction
            </button>
          </div>
          <button @click="goPredictionsList(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
            View predictions
          </button>
          <span v-if="row.isCompleted && row.teamAScore !== null && row.teamBScore !== null" class="text-green-500">
              ✔️
            </span>
        </template>
      </UTable>
    </div>
    </div>
    </div>
  </template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import AddGameEvent from '@/components/AddGameEvent.vue';
import { useGameEventsStore } from '@/stores/stores';
import { format } from 'date-fns';
import { usePredictionGameStore } from '@/stores/stores';
const predictionGameStore = usePredictionGameStore();
const uniqueCode = ref('');
const showModal = ref(false);
const gameEventStore = useGameEventsStore();
const { gameEvents } = storeToRefs(gameEventStore);
const router = useRouter();
const userStore = useUserStore();
const predictionStore = usePredictionsStore();
const copySuccess = ref(false);
const copyToClipboard = async () => {
  try {
    await navigator.clipboard.writeText(uniqueCode.value);
    copySuccess.value = true;
    setTimeout(() => (copySuccess.value = false), 2000);
  } catch (err) {
    console.error('Failed to copy: ', err);
  }
};

const props = defineProps<{
  title: string;
  predictionGameId: number;
}>();

const columns = [
  {
    key: "teams",
    label: "Teams",
  },
  {
    key: "eventDate",
    label: "Event date",
  },
  {
    key: "teamAScore",
    label: "Team A score",
  },
  {
    key: "teamBScore",
    label: "Team B score",
  },
  {
    key: "actions",
    label: "Actions"
  }
];

const hasMadePredictionMap = ref<{ [key: number]: boolean }>({});
const isGameCreator = ref(false);
const predictionGameCreatorId = ref<number | null>(null);

onMounted(async () => {
  const gameData = await predictionGameStore.loadPredictionGame(props.predictionGameId);
  uniqueCode.value = gameData?.uniqueCode || "Not available";
  await gameEventStore.loadGameEvents(props.predictionGameId);
  const userId = userStore.user?.id;

  const predictionGame = await predictionGameStore.getPredictionGameById(props.predictionGameId);
  if (predictionGame) {
    predictionGameCreatorId.value = predictionGame.gameCreatorId; // Assuming this exists
    const userId = userStore.user?.id;
    isGameCreator.value = userId === predictionGameCreatorId.value;
  }
  
  // Check if the user has made a prediction for each game event
  if (userId) {
    for (const event of gameEvents.value) {
      const hasMadePrediction = await userHasMadePrediction(event, userId);
      hasMadePredictionMap.value[event.id] = hasMadePrediction;
    }
  }
});


async function userHasMadePrediction(gameEvent: GameEvent, userId: number): Promise<boolean> {
  const predictions = await predictionStore.getPredictions(gameEvent.id);
  return predictions.some(element => element.predictionMakerId === userId);
}

const formattedGameEvents = computed(() => {
  return gameEvents.value.map(event => ({
    ...event,
    teams: `${event.teamA} \n ${event.teamB}`,
    eventDate : event.eventDate ? format(new Date(event.eventDate), 'dd.MM.yyyy HH:mm') : '',
  }));
});


const deletePredictionGameEvent = (gameEvent: GameEvent) => {
  gameEventStore.deletePredictionGameEvent(gameEvent);
};

const goToEditPredictionGameEvent = (gameEvent: GameEvent) => {
  router.push(`/edit-gameevent/${props.predictionGameId}/${gameEvent.id}`)
}

const goToPredictionGameEventDetails = (gameEvent: GameEvent) => {
  router.push(`/gameevent-details/${props.predictionGameId}/${gameEvent.id}`)
}

const closeModal = () => {
  showModal.value = false; // Sulge modaal
};

const goToMakingAPrediction = (gameEvent: GameEvent) => {
  router.push(`/add-prediction/${props.predictionGameId}/${gameEvent.id}`)
}

const goPredictionsList = (gameEvent: GameEvent) => {
  router.push(`/predictions/${props.predictionGameId}/${gameEvent.id}`)
}

</script>

<style scoped>
.unique-code-box {
  width: 185px;
  padding: 1rem;
}
.copy-button:hover {
  background: linear-gradient(to right, #444242, #9b9494);
}
.copy-button {
  color: rgb(255, 255, 255);
  font-size: 1rem;
  padding: 0.35rem 0.75rem;
}
</style>