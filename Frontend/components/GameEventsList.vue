<template>
  <div class="min-h-screen bg-white dark:bg-gray-900 flex justify-center items-start">
    <div class="bg-white dark:bg-gray-800 rounded-lg shadow-lg max-w-6xl w-full relative">
      
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
            @close="closeModal" />
        
        <div v-if="uniqueCode && uniqueCode !== 'Not available'" class="unique-code-box absolute top-14 right-5">
          <p class="text-2xl font-bold mr-2">{{ uniqueCode }}</p>
          <button @click="copyToClipboard" class="copy-button">
            <i class="fas fa-copy mr-1"></i>
          </button>
        </div>
        <div class="mt-8">
          <h1 class="text-3xl font-bold text-center mb-6 text-black dark:text-white">{{ title }}</h1>
          <UTable class="w-full"
          :rows="formattedGameEvents" :columns="columns">

          <template #empty-state>
            <div class="flex flex-col items-center justify-center py-6 gap-3">
              <span class="italic text-base">No events here, create now! </span>
            </div>
          </template>
    
        <template #teams-data="{ row }">
          <div v-html="row.teams.replace('\n', '<br>')"></div>
        </template>

        <template #actions-data="{ row }">
              <div class="flex justify-end space-x-4 items-center">
                <div class="flex flex-col space-y-2">
                  <div v-if="hasMadePredictionMap[row.id] === true">
                    <button class="bg-gray-500 text-white rounded-[80px] px-4 py-1.5">
                      Prediction made
                    </button>
                  </div>
                  <div v-else>
                    <button @click="showModalPrediction[row.id] = true" 
                    class="btn-primary-small">
                      Make a prediction
                    </button>

                    <AddPrediction 
                      v-if="showModalPrediction[row.id]" 
                      :predictionGameId="props.predictionGameId" 
                      :game-event-id="row.id"
                      @close="closeModalPrediction(row.id)"
                      @refresh="fetchData"/>

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
        <button 
        v-if="!isChatOpen" @click="toggleChat" class="chat-toggle-button">
        <i class="fas fa-comments"></i>
        </button>


    <!-- Chat Window -->
    <div v-if="isChatOpen" class="chat-box-container">
  <div class="chat-header">
    <h2 class="text-xl font-bold mb-2 text-black dark:text-white">Chat</h2>
    <button @click="toggleChat" class="close-button">-</button>
  </div>
  <!-- Add the closing div for chat-header -->
  <div v-if="!isChatMinimized" class="chat-content">
    <ChatBox 
      :gameId="props.predictionGameId" 
      :currentUserId="userStore.user?.id" 
    />
  </div>
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
import ChatBox from '@/components/ChatBox.vue';

const selectedEventId = ref<number | null>(null);
  const toggleChatBox = (eventId: number) => {
  if (selectedEventId.value === eventId) {
    selectedEventId.value = null;
  } else {
    selectedEventId.value = eventId;
  }
};
const isChatMinimized = ref(false);

const minimizeChat = () => {
  isChatMinimized.value = !isChatMinimized.value;
};
const isChatOpen = ref(false);
const toggleChat = () => {
  isChatOpen.value = !isChatOpen.value;
};
const predictionGameStore = usePredictionGameStore();
const uniqueCode = ref('');
const showModal = ref(false);
const showModalPrediction = ref<{ [key: number]: boolean }>({});


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
    key: "eventDate",
    label: "Event date and time",
  },
  {
    key: "teams",
    label: "Teams",
  },
  {
    key: "score",
    label: "Score",
  },
  {
    key: "yourPrediction",
    label: "Your prediction"
  },
  {
    key: "actions",
    label: ""
  }
];

const hasMadePredictionMap = ref<{ [key: number]: boolean }>({});
const isGameCreator = ref(false);
const predictionGameCreatorId = ref<number | null>(null);

const formattedGameEvents = computed(() => {
  return gameEvents.value.map( (event) => {
    const userPrediction = predictionStore.userPredictionsMap[event.id];

    return {
      ...event,
      teams: `${event.teamA} \n ${event.teamB}`,
      score: event.teamAScore !== null || event.teamBScore !== null
        ? `${event.teamAScore} - ${event.teamBScore}`
        : "",
      yourPrediction: userPrediction
        ? `${userPrediction.endScoreTeamA} - ${userPrediction.endScoreTeamB}`
        : "No prediction made",
      eventDate: event.eventDate
        ? format(new Date(event.eventDate), 'dd.MM.yyyy HH:mm')
        : '',
    };
  });
});


async function fetchData() {
  const gameData = await predictionGameStore.loadPredictionGame(props.predictionGameId);
  uniqueCode.value = gameData?.uniqueCode || "Not available";

  await gameEventStore.loadGameEvents(props.predictionGameId);
  const userId = userStore.user?.id;

  const predictionGame = await predictionGameStore.getPredictionGameById(props.predictionGameId);
  if (predictionGame) {
    predictionGameCreatorId.value = predictionGame.gameCreatorId;
    isGameCreator.value = userId === predictionGameCreatorId.value;
  }
  
  if (userId) {
    const predictionsMap: { [key: number]: Prediction | null } = {};
    await Promise.all(gameEvents.value.map(async (event) => {
      await predictionStore.loadUserPrediction(event.id);
      predictionsMap[event.id] = predictionStore.userPrediction ? 
        { ...(predictionStore.userPrediction as Prediction) } : null;
    }));
    for (const event of gameEvents.value) {
      const hasMadePrediction = await userHasMadePrediction(event, userId);
      hasMadePredictionMap.value[event.id] = hasMadePrediction;
    }
    predictionStore.userPredictionsMap = predictionsMap;
  }
}


onMounted(async () => {
  fetchData()
});

async function userHasMadePrediction(gameEvent: GameEvent, userId: number): Promise<boolean> {
  await predictionStore.loadPredictions(gameEvent.id);
  return predictionStore.predictions.some(element => element.predictionMakerId === userId);
}

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

const closeModalPrediction = (eventId: number) => {
  showModalPrediction.value[eventId] = false; 
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
  background-color: #26547C;
  transform: scale(1.05);
}

.copy-button:active {
  background-color: #26547C;
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
  background-color: #26547C;
  transform: scale(1.05);
}

.btn-primary-large:active {
  background-color: #26547C;
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
  background-color: #26547C;
  transform: scale(1.05);
}

.btn-primary-small:active {
  background-color: #26547C;
  transform: scale(1);
}

h2 {
  color: #757575;
}

h1 {
  color: #2c3e50;
}
.chat-box-container {
  position: fixed;
  bottom: 20px;
  right: 20px;
  width: 300px;
  background-color: #ffffff;
  border: 1px solid #ccc;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  z-index: 1000;
  padding: 10px;
}
.chat-toggle-button {
  position: fixed;
  bottom: 20px;
  right: 20px;
  width: 50px;
  height: 50px;
  background-color: #f1f1f1;
  border-radius: 50%;
  border: none;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
  transition: background-color 0.3s ease;
  z-index: 1100;
}

.chat-toggle-button:hover {
  background-color: #333;
}

.chat-toggle-button i {
  font-size: 20px;
  color: #333;
}

.chat-header-wrapper {
  position: relative; 
  background-color: #ffffff; 
  padding: 10px;
  border-bottom: 1px solid #ccc;
}

.chat-header {
  display: flex;
  align-items: center;
  justify-content: space-between; 
}

.minimize-button,
.close-button {
  position: absolute;
  top: 1px;
  background: none;
  border: none;
  color: black; 
  font-size: 24px;
  cursor: pointer;
  z-index: 1200; 
}


.close-button {
  left: 20px; 
}

</style>
