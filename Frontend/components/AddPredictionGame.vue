<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Nimetus" name="predictionGameTitle">
        <UInput v-model="state.predictionGameTitle" />
      </UFormGroup>
  
      <UFormGroup label="Alguskuupäev" name="startDate">
        <UInput v-model="startDateStr" type="date" />
      </UFormGroup>
  
      <UFormGroup label="Lõppkuupäev" name="endDate">
        <UInput v-model="endDateStr" type="date" />
      </UFormGroup>
  
      <UFormGroup label="Privaatsus" name="privacy">
        <UInput v-model="state.privacy" />
      </UFormGroup>
  
      <UButton type="submit">Lisa</UButton>
    </UForm>
  </template>
  
  <script setup lang="ts">
    import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  
    const { addPredictionGame } = usePredictionGameStore();
  
    const state = reactive<PredictionGame>({
      id: 0,
      predictionGameTitle: '', 
      creationDate: new Date(),
      startDate: '', 
      endDate: '', 
      gameCreatorId: 1, 
      privacy: 'Private game'
    });
  
    const startDateStr = computed({
        get: () => (state.startDate instanceof Date ? state.startDate.toISOString().split('T')[0] : state.startDate),
        set: (value: string) => {
            state.startDate = value;
        }
    });

    const endDateStr = computed({
        get: () => (state.endDate instanceof Date ? state.endDate.toISOString().split('T')[0] : state.endDate),
        set: (value: string) => {
            state.endDate = value;
        }
    });
  
    const validate = (state: any): FormError[] => {
      const errors = [];
      if (!state.predictionGameTitle) errors.push({ path: "predictionGameTitle", message: "Required" });
      if (!state.startDate) errors.push({ path: "startDate", message: "Required" });
      if (!state.endDate) errors.push({ path: "endDate", message: "Required" });
      return errors;
    };
  
    async function onSubmit(event: FormSubmitEvent<any>) {
        const payload = {
            id: state.id,
            predictionGameTitle: state.predictionGameTitle,
            creationDate: state.creationDate,
            startDate: startDateStr.value,
            endDate: endDateStr.value,
            gameCreatorId: state.gameCreatorId,
            privacy: state.privacy
        };
      addPredictionGame(payload);
      await navigateTo("/predictiongames");
    }
  
    async function onError(event: FormErrorEvent) {
      const element = document.getElementById(event.errors[0].id);
      element?.focus();
      element?.scrollIntoView({ behavior: "smooth", block: "center" });
    }
  </script>
  