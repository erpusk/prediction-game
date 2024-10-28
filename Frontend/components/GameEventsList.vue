<template>
  <div class="min-h-screen bg-white flex justify-center items-center p-6">
  <div class="p-20 bg-white rounded-lg shadow-lg max-w-5x1">
    <button 
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

      <div v-if="gameEvents.length === 0">
        <h2 class="text-x1 text-center">No events have been added</h2>
      </div>
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black" >{{ title }}</h1>
        <UTable :rows="gameEvents" :columns="columns">
          <template #actions-data="{ row }">
              <button @click="deletePredictionGameEvent(row)" class="flex items-center text-red-500 hover:text-red-700">
                <DeleteIconComponent />
              </button>
              <button @click="goToEditPredictionGameEvent(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
                Edit
              </button>
              <button @click="goToPredictionGameEventDetails(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
                Details
              </button>
              <button @click="goToMakingAPrediction(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
                Make a prediction
              </button>
          </template>
        </UTable>
      </div>
    </div>
<<<<<<< HEAD
=======
    <div v-else class="mt-8">
      <h1 class="text-3xl font-bold text-center mb-6 text-black">{{ title }}</h1>
      <UTable :rows="gameEvents" :columns="columns">
        <template #actions-data="{ row }">
          <button @click="deletePredictionGameEvent(row)" class="flex items-center text-red-500 hover:text-red-700">
            <DeleteIconComponent />
          </button>
          <button @click="goToEditPredictionGameEvent(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
            Edit
          </button>
          <button @click="goToPredictionGameEventDetails(row)" class="bg-blue-500 text-white rounded px-4 py-2 hover:bg-blue-600">
            Details
          </button>
          <span v-if="row.isCompleted && row.teamAScore !== null && row.teamBScore !== null" class="text-green-500">
              ✔️
            </span>
        </template>
      </UTable>
>>>>>>> 445d3a8ef901817c01d533565bd6d82c8041de90
    </div>
  </template>


<script setup lang="ts">
import { ref, onMounted } from 'vue';
import AddGameEvent from '@/components/AddGameEvent.vue';
import { useGameEventsStore } from '@/stores/stores';
const showModal = ref(false);
import PredictionGameId from '~/pages/add-gameevents/[predictionGameId].vue';
const props = defineProps<{
  title: string;
  predictionGameId: string | string[];
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
    label: "Actions"
  }
];

const gameEventStore = useGameEventsStore();
const { gameEvents } = storeToRefs(gameEventStore);
const route = useRoute();
const router = useRouter();

const goToCreateNewPredictionGameEvent = (predictionGameId: string | string[]) => {
  router.push(`/add-gameevents/${predictionGameId}`);
};

onMounted(() => {
  gameEventStore.loadGameEvents(props.predictionGameId);
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
</script>