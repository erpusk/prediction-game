<template>
    <UForm
      :validate="validate"
      :state="state"
      class="p-6 bg-white rounded-lg shadow-lg"
      @submit="onSubmit"
      @error="onError"
    >
      <h2 class="text-2xl font-semibold text-center mb-4 text-black">Edit an event</h2>
      <UFormGroup label="Esimene meeskond" name="teamA">
        <UInput v-model="state.teamA" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Teine meeskond" name="teamB">
        <UInput v-model="state.teamB" class="border rounded-md p-2"/>
      </UFormGroup>
  
      <UFormGroup label="Toimumisaeg" name="eventDate">
        <UInput v-model="eventDateStr" type="date" class="border rounded-md p-2"/>
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
  </template>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useGameEventsStore } from "~/stores/stores";
    import type { GameEvent } from "~/types/gameEvent";

    const { editPredictionGameEvent } = useGameEventsStore();
    const {loadSingleEvent} = useGameEventsStore();
    

    const props = defineProps<{
      predictionGameId: string | string[],
      gameEventId: string | string[];
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
        get: () => (state.eventDate instanceof Date ? state.eventDate.toISOString().split('T')[0] : state.eventDate),
        set: (value: string) => {
            state.eventDate = value;
        }
    });

    onMounted(async () => {
  const gameEventData = await loadSingleEvent(props.gameEventId.toString());
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
      return errors;
    };
  
    async function onSubmit(gameEvent: FormSubmitEvent<any>) {
        const payload = {
            
            
            id: parseInt(props.gameEventId.toString()),
            teamA: state.teamA,
            teamB: state.teamB,
            eventDate: eventDateStr.value,
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