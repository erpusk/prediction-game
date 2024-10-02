<template>
    <UForm
      :validate="validate"
      :state="state"
      class="space-y-4"
      @submit="onSubmit"
      @error="onError"
    >
      <UFormGroup label="Nimetus" name="title">
        <UInput v-model="state.title" />
      </UFormGroup>
  
      <UFormGroup label="Kirjeldus" name="description">
        <UInput v-model="state.description" type="description" />
      </UFormGroup>
  
      <UButton type="submit"> Lisa </UButton>
    </UForm>
</template>
  
<script setup lang="ts">
  import type { FormError, FormErrorEvent, FormSubmitEvent } from "#ui/types";
  
  const { addExercise } = useWorkoutStore();

  const state = reactive<Exercise>({
    title: undefined,
    description: undefined,
  });
  
  const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.title) errors.push({ path: "title", message: "Required" });
    if (!state.description)
      errors.push({ path: "description", message: "Required" });
    return errors;
  };
  
  async function onSubmit(event: FormSubmitEvent<any>) {
    addExercise({ ...state });
    await navigateTo("/exercises");
  }
  
  async function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
</script>  