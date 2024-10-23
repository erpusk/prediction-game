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
        <UFormGroup label="Esimene meeskond" name="teamA">
          <UInput v-model="state.teamA" class="border rounded-md p-2 w-full"/>
        </UFormGroup>
    
        <UFormGroup label="Teine meeskond" name="teamB">
          <UInput v-model="state.teamB" class="border rounded-md p-2 w-full"/>
        </UFormGroup>
    
        <UFormGroup label="Toimumisaeg" name="eventDate">
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

  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useGameEventsStore } from "~/stores/stores";
    import type { GameEvent } from "~/types/gameEvent";
    const emit = defineEmits(['close']);
    const { addGameEvent } = useGameEventsStore();

    const props = defineProps<{
      predictionGameId: string | string[];
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
  
    const validate = (state: any): FormError[] => {
      const errors = [];
      if (!state.teamA) errors.push({ path: "teamA", message: "Required" });
      if (!state.teamB) errors.push({ path: "teamB", message: "Required" });
      if (!state.eventDate) errors.push({ path: "eventDate", message: "Required" });
      return errors;
    };
  
    async function onSubmit(gameEvent: FormSubmitEvent<any>) {
        const payload = {
            id: state.id,
            teamA: state.teamA,
            teamB: state.teamB,
            eventDate: eventDateStr.value,
            predictionGameId: parseInt(props.predictionGameId.toString(), 10),
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