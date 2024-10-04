<template>
    <div>
      <h1 class="text-xl text-center">{{ title }}</h1>
  
      <div v-if="predictionGames.length === 0">
        <h2 class="text-x1 text-center">Prediction games are missing</h2>
      </div>
      <div v-else>
        <UTable :rows="predictionGames" :columns="columns">
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
  import { usePredictionGameStore } from '@/stores/stores';
  import DeleteIconComponent from '@/components/DeleteIconComponent.vue';
  
  defineProps<{ title: string }>();
  
  const columns = [
    {
      key: "predictionGameTitle",
      label: "Title",
    },
    {
      key: "privacy",
      label: "Privacy",
    },
    {
      key: "startDate",
      label: "Start Date",
    },
    {
      key: "endDate",
      label: "End Date",
    },
    {
      key: "actions",
      label: "Actions",
    },
  ];
  
  const predictionGameStore = usePredictionGameStore();
  const { predictionGames } = storeToRefs(predictionGameStore);
  
  onMounted(() => {
    predictionGameStore.loadPredictionGames();
  });
  
//   const deletePredictionGame = (game: PredictionGame) => {
//     predictionGameStore.deletePredictionGame(game);
//   };
  </script>
  