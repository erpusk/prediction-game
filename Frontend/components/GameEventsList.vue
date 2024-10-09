<template>
    <div class="p-6 bg-white rounded-lg shadow-lg">

      <div v-if="gameEvents.length === 0">
        <h2 class="text-x1 text-center">No events have been added</h2>
      </div>
      <div v-else class="mt-8">
        <h1 class="text-3xl font-bold text-center mb-6 text-black" >{{ title }}</h1>
        <UTable :rows="gameEvents" :columns="columns">
          <!--<template #actions-data="{ row }">
              <button @click="deletePredictionGame(row)">
                  <DeleteIconComponent />
              </button>
          </template>-->
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
  }
];

const gameEventStore = useGameEventsStore();
const { gameEvents } = storeToRefs(gameEventStore);
const route = useRoute();



onMounted(() => {
  gameEventStore.loadGameEvents(props.predictionGameId);
});

//   const deletePredictionGame = (game: PredictionGame) => {
//     predictionGameStore.deletePredictionGame(game);
//   };
</script>