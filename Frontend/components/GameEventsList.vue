<template>
  <div class="min-h-screen bg-white flex justify-center items-start mt-20">
    <div class="bg-white rounded-lg shadow-lg max-w-6xl w-full relative">
      
      <div class="relative">
        <button 
          v-if="isGameCreator"
          @click="showModal = true" 
          class="btn-primary-large absolute top-0 right-5">
          Create a new Event
        </button>
          <AddGameEvent 
            v-if="showModal" 
            :predictionGameId="props.predictionGameId" 
            @close="closeModal" 
          />
        
        <div v-if="uniqueCode && uniqueCode !== 'Not available'" class="unique-code-box absolute top-16 right-5">
          <p class="text-2xl font-bold mr-2">{{ uniqueCode }}</p>
          <button @click="copyToClipboard" class="copy-button">
            <i class="fas fa-copy mr-1"></i>
          </button>
        </div>

      <div v-if="gameEvents.length === 0">
        <h2 class="text-xl text-center">No events have been added</h2>
      </div>
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black">{{ title }}</h1>
          <UTable :rows="formattedGameEvents" :columns="columns">
            <template #actions-data="{ row }">
              <div class="flex justify-end space-x-4 items-center">
                <div class="flex flex-col space-y-2">
                  <div v-if="hasMadePredictionMap[row.id] === true">
                    <button class="bg-gray-500 text-white rounded-[80px] px-4 py-1.5">
                      Prediction made
                    </button>
                  </div>
                  <div v-else>
                    <button @click="goToMakingAPrediction(row)" class="btn-primary-small">
                      Make a prediction
                    </button>
                  </div>
                  <button @click="goPredictionsList(row)" class="btn-primary-small">
                    View predictions
                  </button>
                </div>

                <button @click="goToPredictionGameEventDetails(row)" class="btn-primary-small">
                  Details
                </button>

                <template v-if="isGameCreator">
                  <button @click="goToEditPredictionGameEvent(row)" class="btn-primary-small">
                    Edit
                  </button>
                  <button @click="deletePredictionGameEvent(row)" class="flex items-center text-red-500 hover:text-red-700">
                    <DeleteIconComponent />
                  </button>
                </template>
              </div>
            </template>
          </UTable>
        </div>
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
    key: "teamA",
    label: "Team A",
  },
  {
    key: "teamB",
    label: "Team B",
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
    label: ""
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
    predictionGameCreatorId.value = predictionGame.gameCreatorId;
    const userId = userStore.user?.id;
    isGameCreator.value = userId === predictionGameCreatorId.value;
  }
  
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
  showModal.value = false; 
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
  width: auto;
  display: flex;
  justify-content: space-between;
  align-items: center;
  z-index: 10;
}

.copy-button {
  padding: 8px;
  font-size: 14px;
  color: #fff;
  background-color: #5bb17c;
  border-radius: 50%;
  border: none;
  cursor: pointer;
  transition: background-color 0.3s, transform 0.3s;
}

.copy-button:hover {
  background-color: #4d6bac;
  transform: scale(1.05);
}

.copy-button:active {
  background-color: #3f6b96;
  transform: scale(1);
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
  background-color: #4d6bac;
  transform: scale(1.05);
}

.btn-primary-large:active {
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

h2 {
  color: #757575;
}

h1 {
  color: #2c3e50;
}
</style>
