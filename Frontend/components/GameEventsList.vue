<template>
    <div class="p-6 bg-white rounded-lg shadow-lg">
      <button 
        @click="goToCreateNewPredictionGameEvent(props.predictionGameId)" 
        class="absolute top-20 right-10 bg-gradient-to-r from-blue-500 to-blue-700 hover:from-blue-600 hover:to-blue-800 text-white font-semibold py-3 px-6 rounded-md shadow-md hover:shadow-lg transition duration-300 ease-in-out transform hover:scale-105">
        Create a new Event
      </button>

      <div v-if="gameEvents.length === 0">
        <h2 class="text-x1 text-center">No events have been added</h2>
      </div>
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black" >{{ title }}</h1>
        <UTable :rows="gameEvents" :columns="columns">
          <template #actions-data="{ row }">
              <button @click="deletePredictionGameEvent(row)">
                  <DeleteIconComponent />
              </button>
          </template>
        </UTable>
      </div>
    </div>
  </template>

<script setup lang="ts">
import { useGameEventsStore } from '@/stores/stores';
const props = defineProps<{
  title: string;
  predictionGameId: string | string[]; // Accept predictionGameId as a prop
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
</script>