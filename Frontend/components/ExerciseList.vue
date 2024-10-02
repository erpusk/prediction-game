<template>
    <div>
      <h1 class="text-xl text-center">{{ title }}</h1>
  
      <!-- Display "Harjutused puuduvad" if no exercises exist -->
      <div v-if="exercises.length === 0">
        <h2 class="text-x1 text-center">Harjutused puuduvad</h2>
      </div>
      <div v-else>
        <UTable :rows="exercises" :columns="columns">
        
        <!--<template #actions-data="{ row }">
            <button @click="deleteExercise(row)">
                <DeleteIconComponent />
            </button>
        </template>-->

         </UTable>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { useWorkoutStore } from '@/stores/stores';
  import DeleteIconComponent from '@/components/DeleteIconComponent.vue';
  
  defineProps<{ title: string }>();
  
  const columns = [
    {
      key: "title",
      label: "Nimetus",
    },
    {
      key: "description",
      label: "Kirjeldus",
    },
    {
    key: "actions",
    label: "Tegevused",
    },
  ];
  
  const workoutStore = useWorkoutStore();
  const { exercises } = storeToRefs(workoutStore);
  
  onMounted(() => {
  workoutStore.loadExercises();
  });

  //const deleteExercise = (exercise: Exercise) => {
  //workoutStore.deleteExercise(exercise);
  //};
  </script>  