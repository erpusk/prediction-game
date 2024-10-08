<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-6 p-6 bg-white rounded-lg shadow-lg"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4">Create a Prediction Game</h2>
      
      <UFormGroup label="Prediction game title" name="predictionGameTitle">
        <UInput v-model="state.predictionGameTitle" placeholder="Enter game title" class="border rounded-md p-2" />
      </UFormGroup>
  
      <UFormGroup label="Start date" name="startDate">
        <UInput v-model="startDateStr" type="date" class="border rounded-md p-2" />
      </UFormGroup>
  
      <UFormGroup label="End date" name="endDate">
        <UInput v-model="endDateStr" type="date" class="border rounded-md p-2" />
      </UFormGroup>
  
      <UFormGroup label="Game privacy" name="privacy">
        <USelect v-model="state.privacy" :options="['Private game', 'Public game']" />
      </UFormGroup>
  
      <div class="flex justify-center space-x-4 mt-6">
        <UButton type="button" @click="navigateToListOfPredictionGames" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
            Back to List
        </UButton>
        <UButton type="submit" class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md transition duration-300">
            Create a Game
        </UButton>
      </div>
    </UForm>
  </template>
  
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useRouter } from 'vue-router';

  
    const { addPredictionGame } = usePredictionGameStore();
  
    const state = reactive<PredictionGame>({
      id: 0,
      predictionGameTitle: '', 
      creationDate: new Date(),
      startDate: '', 
      endDate: '', 
      gameCreatorId: 1, 
      privacy: 'Private game'
    });
  
    const startDateStr = computed({
        get: () => (state.startDate instanceof Date ? state.startDate.toISOString().split('T')[0] : state.startDate),
        set: (value: string) => {
            state.startDate = value;
        }
    });

    const endDateStr = computed({
        get: () => (state.endDate instanceof Date ? state.endDate.toISOString().split('T')[0] : state.endDate),
        set: (value: string) => {
            state.endDate = value;
        }
    });
  
    const validate = (state: any): FormError[] => {
      const errors = [];
      if (!state.predictionGameTitle) errors.push({ path: "predictionGameTitle", message: "Required" });
      if (!state.startDate) errors.push({ path: "startDate", message: "Required" });
      if (!state.endDate) errors.push({ path: "endDate", message: "Required" });
      return errors;
    };
  
    async function onSubmit(event: FormSubmitEvent<any>) {
        const payload = {
            id: state.id,
            predictionGameTitle: state.predictionGameTitle,
            creationDate: state.creationDate,
            startDate: startDateStr.value,
            endDate: endDateStr.value,
            gameCreatorId: state.gameCreatorId,
            privacy: state.privacy
        };
      addPredictionGame(payload);
      await navigateTo("/predictiongames");
    }

    const router = useRouter();

    const navigateToListOfPredictionGames = () => {
    router.push('/predictiongames');
    };
  
    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>
  