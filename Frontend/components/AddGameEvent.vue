<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Esimene meeskond" name="teamA">
        <UInput v-model="state.teamA" />
      </UFormGroup>
  
      <UFormGroup label="Teine meeskond" name="teamB">
        <UInput v-model="state.teamB" />
      </UFormGroup>
  
      <UFormGroup label="Toimumisaeg" name="eventDate">
        <UInput v-model="eventDateStr" type="date" />
      </UFormGroup>
  
      <UButton type="submit">Lisa</UButton>
    </UForm>
  </template>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
    import { useGameEventsStore } from "~/stores/stores";
    import type { GameEvent } from "~/types/gameEvent";

    const { addGameEvent } = useGameEventsStore();

    const props = defineProps<{
      predictionGameId: string | string[]; // Accept predictionGameId as a prop
    }>();

    const state = reactive<GameEvent>({
        id: 0,
        teamA: '',
        teamB: '',
        eventDate: new Date(),
        predictionGameId: 0,
        teamAScore: null,
        teamBScore: null,
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
      await navigateTo(`/gameevents/${props.predictionGameId}`);
    }
  
    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>