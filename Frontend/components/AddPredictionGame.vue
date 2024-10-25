<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-6 p-6 bg-white rounded-lg shadow-lg"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black">Create a Prediction Game</h2>
      
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
    get: () => {
        if (state.startDate) {
            return (new Date(state.startDate)).toISOString().split('T')[0];
        }
        return ''; // Tagasta tühi string, kui kuupäeva pole
    },
    set: (value: string) => {
        if (value) {
            state.startDate = new Date(value).toISOString();  // Määrame kuupäeva ISO formaadis
        } else {
            state.startDate = ''; // Tühjenda väärtus, kui kuupäev on määramata
        }
    }
});

const endDateStr = computed({
    get: () => {
        if (state.endDate) {
            return (new Date(state.endDate)).toISOString().split('T')[0];
        }
        return '';
    },
    set: (value: string) => {
        if (value) {
            state.endDate = new Date(value).toISOString();
        } else {
            state.endDate = '';
        }
    }
});
  
    const validate = (state: any): FormError[] => {
      const errors = [];
      const currentDate = new Date();

      // validate prediction game title
      if (!state.predictionGameTitle) errors.push({ path: "predictionGameTitle", message: "Required" });
      else if (state.predictionGameTitle.length < 4 || state.predictionGameTitle.length > 40) {
        errors.push({ path: "predictionGameTitle", message: "Title must be between 4 and 40 characters." });
      }

      // validate start and end dates
      if (!state.startDate) errors.push({ path: "startDate", message: "Required" });
      if (!state.endDate) errors.push({ path: "endDate", message: "Required" });
      if (new Date(state.startDate) >= new Date(state.endDate)) {
        errors.push({ path: "endDate", message: "End date must be after the start date." });
      }
      // Check if startDate and endDate are in the future
      if (state.startDate && new Date(state.startDate) <= currentDate) {
        errors.push({ path: "startDate", message: "Start date must be in the future." });
      }
      if (state.endDate && new Date(state.endDate) <= currentDate) {
          errors.push({ path: "endDate", message: "End date must be in the future." });
      }

      // validate privacy field
      if (!state.privacy) errors.push({ path: "privacy", message: "Please select a game privacy option." });

      return errors;
    };
  
    async function onSubmit(event: FormSubmitEvent<any>) {
        const payload = {
            id: state.id,
            predictionGameTitle: state.predictionGameTitle,
            creationDate: new Date().toISOString(),
            startDate: startDateStr.value ? new Date(startDateStr.value).toISOString() : undefined,
            endDate: endDateStr.value ? new Date(endDateStr.value).toISOString() : undefined,
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
  