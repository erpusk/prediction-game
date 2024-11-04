<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
    <div class="bg-white rounded-lg shadow-lg p-6 w-1/3 relative">
      <button 
        @click="$emit('close')" 
        class="absolute top-2 right-2 text-black hover:text-red-600 transition duration-300">
        &times;
      </button>
      <h2 class="text-2xl font-semibold text-center mb-4 text-black">Add an event</h2>

      <UForm
        :validate="validate"
        :state="state"
        @submit="onSubmit"
        @error="onError"
      >
        <UFormGroup label="Team A" name="teamA" >
          <UInput v-model="state.teamA" class="border rounded-md p-2 w-full"/>
        </UFormGroup>
    
        <UFormGroup label="Team B" name="teamB">
          <UInput v-model="state.teamB" class="border rounded-md p-2 w-full"/>
        </UFormGroup>
    
        <UFormGroup label="Event date" name="eventDate">
          <UInput v-model="eventDateStr" type="date" class="border rounded-md p-2 w-full"/>
        </UFormGroup>

        <div class="flex justify-between mt-6">
          <UButton type="submit" class="bg-blue-500 hover:bg-blue-600 text-white font-bold py-2 px-4 rounded-md transition duration-300">
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
</style>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useGameEventsStore } from "~/stores/stores";
    import type { GameEvent } from "~/types/gameEvent";
    const emit = defineEmits(['close']);
    const { addGameEvent } = useGameEventsStore();

    const props = defineProps<{
      predictionGameId: number; // Accept predictionGameId as a prop
    }>();

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
      get: () => {
        if (state.eventDate instanceof Date) {
            return state.eventDate.toISOString().split('T')[0];
        }
        return state.eventDate;
    },
    set: (value: string) => {
        state.eventDate = value ? new Date(value) : ''; 
    }
    });


  
    const validate = (state: any): FormError[] => {
      const errors = [];
      const currentDate = new Date();
      // Check for required fields
      if (!state.teamA) errors.push({ path: "teamA", message: "Required" });
      if (!state.teamB) errors.push({ path: "teamB", message: "Required" });
      if (!state.eventDate) errors.push({ path: "eventDate", message: "Required" });
      
      // Check team name length
      if (state.teamA.length < 3 || state.teamA.length > 30) {
        errors.push({ path: "teamA", message: "Team A name must be between 3 and 30 characters." });
      }
      if (state.teamB.length < 3 || state.teamB.length > 30) {
        errors.push({ path: "teamB", message: "Team B name must be between 3 and 30 characters." });
      }

      // Check for future date
      if (state.eventDate && new Date(state.eventDate) <= currentDate) {
        errors.push({ path: "eventDate", message: "Event date must be in the future." });
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
            id: state.id,
            teamA: state.teamA,
            teamB: state.teamB,
            eventDate: state.eventDate,
            predictionGameId: props.predictionGameId,
            teamAScore: state.teamAScore,
            teamBScore: state.teamBScore,
            isCompleted: state.isCompleted
        };
      addGameEvent(payload);
      emit('close');
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