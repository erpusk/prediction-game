<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
    <div class="relative">
      
      <button 
        @click="$emit('close')" 
        class="absolute top-2 right-2 text-black hover:text-red-600 transition duration-300">
        &times;
      </button>

    <UForm
      :validate="validate"
      :state="state"
      class=" p-6 bg-white dark:bg-gray-800 rounded-lg shadow-lg max-w-lg w-full "
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black dark:text-white">Add a prediction</h2>
      <UFormGroup :label="teamALabel" name="endScoreTeamA">
        <UInput v-model="state.endScoreTeamA" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup :label="teamBLabel" name="endScoreTeamB">
        <UInput v-model="state.endScoreTeamB" class="border rounded-md p-2"/>
      </UFormGroup>
      
      <div class="flex justify-between mt-6">
          <UButton @click="$emit('close')" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
            Close
          </UButton>
          <UButton type="submit" class="add-button text-white font-bold py-2 px-4 rounded-md transition duration-300">
            Add
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
    import type { FormError, FormErrorEvent } from "#ui/types";
    import type { Prediction } from "~/types/prediction";
import GameEventsList from "./GameEventsList.vue";

    const { addPrediction } = usePredictionsStore();
    const gameEventStore = useGameEventsStore();
    const emit = defineEmits(['close','refresh']);


    const props = defineProps<{
      gameEventId: number;
      predictionGameId: number;
    }>();

    const event = await gameEventStore.loadSingleEvent(props.gameEventId)
    const teamALabel = event.teamA + " end score"
    const teamBLabel = event.teamB + " end score"

    const state = reactive<Prediction>({
        id: 0,
        endScoreTeamA: 0,
        endScoreTeamB: 0,
        predictionMakerId: 0,
        eventId: 0
    });

    const validate = (state: any): FormError[] => {
      const errors = [];
      if (isNaN(parseInt(state.endScoreTeamA))) errors.push({ path: "endScoreTeamA", message: "Needs to be a number" });
      if (isNaN(parseInt(state.endScoreTeamB))) errors.push({ path: "endScoreTeamB", message: "Needs to be a number" });
      if (parseInt(state.endScoreTeamA) < 0) errors.push({ path: "endScoreTeamA", message: "Cannot be negative" });
      if (parseInt(state.endScoreTeamB) < 0) errors.push({ path: "endScoreTeamB", message: "Cannot be negative" });
      return errors;
    };
  
    async function onSubmit() {
        const payload = {
            id: state.id,
            endScoreTeamA: state.endScoreTeamA,
            endScoreTeamB: state.endScoreTeamB,
            predictionMakerId: state.predictionMakerId,
            eventId: props.gameEventId
        };
    const res = await addPrediction(payload);
    if (res.status === 409) {  
      alert("Event has already ended. Cannot add prediction"); 
      console.log(res)
    }
    emit('refresh')
    emit('close')
  }
  
    async function onError(prediction: FormErrorEvent) {
      const element = document.getElementById(prediction.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
</script>

<style>
.add-button{
  background-color: #5bb17c;
}
.add-button:hover{
  background-color: #26547C;
}
</style>