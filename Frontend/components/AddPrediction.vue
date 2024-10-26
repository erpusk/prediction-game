<template>
    <UForm
      :validate="validate"
      :state="state"
      class="p-6 bg-white rounded-lg shadow-lg"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black">Add an event</h2>
      <UFormGroup label="Esimese meeskonna lõpu punktid" name="endScoreTeamA">
        <UInput v-model="state.endScoreTeamA" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Teise meeskonna lõpu punktid" name="endScoreTeamB">
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
  </template>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
import type { Prediction } from "~/types/prediction";

    const { addPrediction } = usePredictionsStore();

    const props = defineProps<{
      gameEventId: number;
      predictionGameId: number
    }>();

    const state = reactive<Prediction>({
        id: 0,
        endScoreTeamA: 0,
        endScoreTeamB: 0,
        predictionMakerId: 0,
        eventId: 0
    });

    const validate = (state: any): FormError[] => {
      const errors = [];
      if (!state.endScoreTeamA) errors.push({ path: "endScoreTeamA", message: "End scores for both teams are required" });
      if (!state.endScoreTeamB) errors.push({ path: "endScoreTeamB", message: "End scores for both teams are required" });
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
    await navigateTo(`/gameevents/${props.predictionGameId}`);
    }
  
    async function onError(prediction: FormErrorEvent) {
      const element = document.getElementById(prediction.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

    const router = useRouter()

    const navigateToGameEvent = () => {
    router.push(`/gameevents/${props.predictionGameId}`);
    };
</script>