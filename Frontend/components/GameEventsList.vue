<template>
    <div>
      <h1 class="text-xl text-center">{{ title }}</h1>
  
      <div v-if="gameEvents.length === 0">
        <h2 class="text-x1 text-center">Prediction games are missing</h2>
      </div>
      <div v-else>
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
defineProps<{ title: string }>();

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

onMounted(() => {
  gameEventStore.loadGameEvents();
});

//   const deletePredictionGame = (game: PredictionGame) => {
//     predictionGameStore.deletePredictionGame(game);
//   };
</script>