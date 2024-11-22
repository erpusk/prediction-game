<template>
  <div class="flex justify-center items-center">
    <button 
      @click="closePopup" 
      class="absolute top-2 right-2 text-black hover:text-red-600 transition duration-300">
      &times;
    </button>
    <UForm
      :validate="validate"
      :state="state"
      class="p-6 bg-white rounded-lg shadow-lg max-w-lg w-full"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black">Add a prediction</h2>
      <UFormGroup :label="teamALabel" name="endScoreTeamA">
        <UInput v-model="state.endScoreTeamA" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup :label="teamBLabel" name="endScoreTeamB">
        <UInput v-model="state.endScoreTeamB" class="border rounded-md p-2"/>
      </UFormGroup>
      
      <div class="flex justify-center space-x-4 mt-6">
        <UButton type="button" @click="navigateToGameEvent" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
            Back to List
        </UButton>
        <UButton type="submit" class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md transition duration-300">
          Add
        </UButton>
      </div>
      
    </UForm>
  </div>
  </template>
   <style scoped>
   div :deep(label) {
     color: black !important;
   }
   </style>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import type { Prediction } from "~/types/prediction";

    const { addPrediction } = usePredictionsStore();
    const gameEventStore = useGameEventsStore();
    const emit = defineEmits(['close']);

    const props = defineProps<{
      gameEventId: number;
      predictionGameId: number;
      isPopup?: boolean;
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
      if (props.isPopup) {
        emit('close');
      } else {
        await navigateTo(`/gameevents/${props.predictionGameId}`);
      }
    }
  
    async function onError(prediction: FormErrorEvent) {
      const element = document.getElementById(prediction.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

    const router = useRouter()

    function navigateToGameEvent() {
      if (props.isPopup) {
        emit('close');
      } else {
        router.push(`/gameevents/${props.predictionGameId}`);
      }
    }
    function closePopup() {
      emit('close');
    }
</script>