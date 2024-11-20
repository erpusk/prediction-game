<template>
  <div class="min-h-screen flex justify-center items-center bg-white dark:bg-gray-900">
    <UForm
      :validate="validate"
      :state="state"
      class="p-6 bg-white rounded-lg shadow-lg max-w-lg w-full dark:bg-gray-800"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black dark:text-white">Edit an event</h2>
      <UFormGroup label="Team A" name="teamA" class="!text-black">
        <UInput v-model="state.teamA" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Team B" name="teamB" class="!text-black">
        <UInput v-model="state.teamB" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Event date and time" name="eventDate" class="!text-black">
        <UInput v-model="eventDateStr" type="datetime-local" class="border rounded-md p-2 w-full"/>
      </UFormGroup>

      <UFormGroup v-if="isPastDate" label="Team A Score" name="teamAScore" class="!text-black" step="1" min="0">
        <UInput v-model="state.teamAScore" type="text" @input="validateNumericInput" class="border rounded-md p-2" placeholder="Enter score for Team A"/>
      </UFormGroup>

      <UFormGroup v-if="isPastDate" label="Team B Score" name="teamBScore" class="!text-black" step="1" min="0">
        <UInput v-model="state.teamBScore" type="text" @input="validateNumericInput" class="border rounded-md p-2" placeholder="Enter score for Team B"/>
      </UFormGroup>

      <UFormGroup v-if="isPastDate" label="Is Completed?" name="isCompleted" class="!text-black">
        <UCheckbox v-model="state.isCompleted" />
      </UFormGroup>
      
      <div class="flex justify-center space-x-4 mt-6">
        <UButton type="button" @click="navigateToListOfGameEvents" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300 dark:hover:bg-gray-500 dark:bg-gray-400">
            Back to List
        </UButton>
        <UButton type="submit" class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md transition duration-300 dark:hover:bg-blue-600 dark:bg-blue-500">
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
  @media (prefers-color-scheme: dark) {
  div :deep(label) {
    color: #ffffff !important;
  }
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

    function validateNumericInput(event: InputEvent) {
      const target = event.target as HTMLInputElement;
      const value = target.value;
      if (isNaN(Number(value)) || !/^\d+$/.test(value)) {
        target.setCustomValidity("Result is not valid");
      } else {
        target.setCustomValidity("");
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
    state.eventDate = gameEventData.eventDate;
    state.predictionGameId = gameEventData.predictionGameId;
    state.teamAScore = gameEventData.teamAScore;
    state.teamBScore = gameEventData.teamBScore;
    state.isCompleted = gameEventData.isCompleted;
  }
});
  
    const validate = (state: any): FormError[] => {
      const errors = [];
      const currentDate = new Date();
      // Check for required fields
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
      
      // Check team name length
      if (state.teamA.length < 3 || state.teamA.length > 30) {
        errors.push({ path: "teamA", message: "Team A name must be between 3 and 30 characters." });
      }
      if (state.teamB.length < 3 || state.teamB.length > 30) {
        errors.push({ path: "teamB", message: "Team B name must be between 3 and 30 characters." });
      }

      // Check for duplicate teams
      if (state.teamA === state.teamB) {
        errors.push({ path: "teamA", message: "Teams must have different names." });
        errors.push({ path: "teamB", message: "Teams must have different names." });
      }
      return errors;
    };
  
    async function onSubmit(gameEvent: FormSubmitEvent<any>) {
        const payload = {
            
            
            id: parseInt(props.gameEventId.toString()),
            teamA: state.teamA,
            teamB: state.teamB,
            eventDate: state.eventDate,
            predictionGameId: props.predictionGameId,
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

    const isPastDate = computed(() => {
      return new Date(state.eventDate) <= new Date();
    });
  </script>