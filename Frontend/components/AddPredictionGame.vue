<template>
  <div class="min-h-screen flex justify-center items-center bg-opacity-50">
    <div class="relative">
    <button 
        @click="$emit('close')" 
        class="absolute top-2 right-2 text-black hover:text-red-600 transition duration-300">
        &times;
    </button>

    <UForm
      :validate="validate"
      :state="state"
      class="space-y-7 px-20 py-10 bg-white dark:bg-gray-800  rounded-lg shadow-lg max-w-lg w-full text-black"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-6 drop-shadow-lg text-black dark:text-white">Create a Prediction Game</h2>
      
      <UFormGroup label="Prediction game title" name="predictionGameTitle" class="!text-black">
        <UInput v-model="state.predictionGameTitle" placeholder="Enter game title" 
         class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Start date" name="startDate" class="!text-black">
        <UInput v-model="startDateStr" type="date"  class="border rounded-md p-2" />
      </UFormGroup>
  
      <UFormGroup label="End date" name="endDate" class="!text-black">
        <UInput v-model="endDateStr" type="date"  class="border rounded-md p-2" />
      </UFormGroup>
  
      <UFormGroup label="Game privacy" name="privacy" class="!text-black">
        <USelect v-model="state.privacy" :options="['Private game', 'Public game']" class="border rounded-md p-2"  />
      </UFormGroup>
  
      <div class="flex justify-between gap-6 mt-6">
        <UButton @click="$emit('close')" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
            Close
        </UButton>
        <UButton type="submit" class="create-button text-white font-bold py-2 px-4 rounded-md transition duration-300">
            Create a Game
        </UButton>
      </div>
    </UForm>
    </div>
  </div>
  </template>
<style scoped>
div :deep(label) {
  color: black !important;
}
@media (prefers-color-scheme: dark) {
  div :deep(label) {
    color: #ffffff !important;
  }
}
</style>

  
  <script setup lang="ts">
    import { _colors } from "#tailwind-config/theme";
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useRouter } from 'vue-router';

    const emit = defineEmits(['close']);
    const { addPredictionGame } = usePredictionGameStore();
    const userStore = useUserStore();
  
    const state = reactive<PredictionGame>({
      id: 0,
      predictionGameTitle: '', 
      creationDate: new Date(),
      startDate: '', 
      endDate: '', 
      gameCreatorId: userStore.user?.id || 0, 
      privacy: 'Private game',
      uniqueCode: '',
      participants:[]
    });
  
    const startDateStr = computed({
    get: () => {
        if (state.startDate) {
            return (new Date(state.startDate)).toISOString().split('T')[0];
        }
        return '';
    },
    set: (value: string) => {
        if (value) {
            state.startDate = new Date(value).toISOString();
        } else {
            state.startDate = '';
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

      if (!state.predictionGameTitle) errors.push({ path: "predictionGameTitle", message: "Required" });
      else if (state.predictionGameTitle.length < 4 || state.predictionGameTitle.length > 40) {
        errors.push({ path: "predictionGameTitle", message: "Title must be between 4 and 40 characters." });
      }

      if (!state.startDate) errors.push({ path: "startDate", message: "Required" });
      if (!state.endDate) errors.push({ path: "endDate", message: "Required" });
      if (new Date(state.startDate) >= new Date(state.endDate)) {
        errors.push({ path: "endDate", message: "End date must be after the start date." });
      }

      if (state.startDate && new Date(state.startDate) <= currentDate) {
        errors.push({ path: "startDate", message: "Start date must be in the future." });
      }
      if (state.endDate && new Date(state.endDate) <= currentDate) {
          errors.push({ path: "endDate", message: "End date must be in the future." });
      }


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
            privacy: state.privacy,
            uniqueCode: state.uniqueCode,
            participants: state.participants
        };
      addPredictionGame(payload);
      emit('close');
    }
  
    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>

<style>
.create-button{
  background-color: #5bb17c;
}
.create-button:hover{
  background-color: #26547C;
}
</style>
  