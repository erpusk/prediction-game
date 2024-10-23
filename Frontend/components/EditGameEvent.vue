<template>
  <div class="min-h-screen flex justify-center items-center bg-white">
    <UForm
      :validate="validate"
      :state="state"
      class="p-6 bg-white rounded-lg shadow-lg max-w-lg w-full"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black">Edit an event</h2>
      <UFormGroup label="Esimene meeskond" name="teamA" class="!text-black">
        <UInput v-model="state.teamA" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Teine meeskond" name="teamB" class="!text-black">
        <UInput v-model="state.teamB" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Toimumisaeg" name="eventDate" class="!text-black">
        <UInput v-model="eventDateStr" type="date" class="border rounded-md p-2"/>
      </UFormGroup>

      <UFormGroup label="Team A Score" name="teamAScore" class="!text-black">
        <UInput v-model="state.teamAScore" type="text" @input="validateNumericInput($event)" class="border rounded-md p-2" placeholder="Enter score for Team A"/>
      </UFormGroup>

      <UFormGroup label="Team B Score" name="teamBScore" class="!text-black">
        <UInput v-model="state.teamBScore" type="text" @input="validateNumericInput($event)" class="border rounded-md p-2" placeholder="Enter score for Team B"/>
      </UFormGroup>

      <UFormGroup label="Is Completed?" name="isCompleted" class="!text-black">
        <UCheckbox v-model="state.isCompleted" />
      </UFormGroup>
      
      <div class="flex justify-center space-x-4 mt-6">
        <UButton type="button" @click="navigateToListOfGameEvents" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
            Back to List
        </UButton>
        <UButton type="submit" class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md transition duration-300">
          Confirm
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
    import { useGameEventsStore } from "~/stores/stores";
    import type { GameEvent } from "~/types/gameEvent";
    import { reactive, computed, onMounted } from 'vue';
    import { useRouter } from 'vue-router';

    const { editPredictionGameEvent } = useGameEventsStore();
    const {loadSingleEvent} = useGameEventsStore();
    
    const props = defineProps<{
      predictionGameId: number,
      gameEventId: number;
    }>();

    function validateNumericInput(event: Event) {
      const input = event.target as HTMLInputElement;
      const value = input.value;
      if (!/^\d*$/.test(value)) {
        input.value = value.replace(/\D/g, '');
      } 
    }

    const state = reactive<GameEvent>({
        id: 0,
        teamA: '',
        teamB: '',
        eventDate: new Date(),
        predictionGameId: 0,
        teamAScore: 0,
        teamBScore: 0,
        isCompleted: false
    });
  
    const eventDateStr = computed({
        get: () => (state.eventDate instanceof Date ? state.eventDate.toISOString().split('T')[0] : state.eventDate),
        set: (value: string) => {
            state.eventDate = value;
        }
    });

    onMounted(async () => {
  const gameEventData = await loadSingleEvent(props.gameEventId);
  if (gameEventData) {
    state.id = gameEventData.id;
    state.teamA = gameEventData.teamA;
    state.teamB = gameEventData.teamB;
    state.eventDate = new Date(gameEventData.eventDate);
    state.predictionGameId = gameEventData.predictionGameId;
    state.teamAScore = gameEventData.teamAScore;
    state.teamBScore = gameEventData.teamBScore;
    state.isCompleted = gameEventData.isCompleted;
  }
});
  
    const validate = (state: any): FormError[] => {
      const errors = [];
      if (!state.teamA) errors.push({ path: "teamA", message: "Required" });
      if (!state.teamB) errors.push({ path: "teamB", message: "Required" });
      if (!state.eventDate) errors.push({ path: "eventDate", message: "Required" });
      if (!state.teamA || state.teamA.trim() === '') {
      errors.push({ path: "teamA", message: "Team A is required" });
      }
      if (!state.teamB || state.teamB.trim() === '') {
      errors.push({ path: "teamB", message: "Team B is required" });
      }
      if (!state.eventDate) {
      errors.push({ path: "eventDate", message: "Event Date is required" });
      }
      if (state.isCompleted) {
      if (state.teamAScore === '' || state.teamAScore === null || isNaN(state.teamAScore)) {
      errors.push({ path: 'teamAScore', message: 'Team A score is required when the event is completed' });
      }
      if (state.teamBScore === '' || state.teamBScore === null || isNaN(state.teamBScore)) {
      errors.push({ path: 'teamBScore', message: 'Team B score is required when the event is completed' });
      }
      }
      return errors;
    };
  
    async function onSubmit(gameEvent: FormSubmitEvent<any>) {
        const payload = {
            
            
            id: parseInt(props.gameEventId.toString()),
            teamA: state.teamA,
            teamB: state.teamB,
            eventDate: state.eventDate,
            predictionGameId: parseInt(props.predictionGameId.toString()),
            teamAScore: state.teamAScore,
            teamBScore: state.teamBScore,
            isCompleted: state.isCompleted
        };
      editPredictionGameEvent(payload);
      await navigateTo(`/gameevents/${props.predictionGameId}`);
    }
  
    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }

    const router = useRouter()

    const navigateToListOfGameEvents = () => {
    router.push(`/gameevents/${props.predictionGameId}`);
    };
  </script>