<template>
    <div class="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center z-50">
      <div class="relative">
        <button 
          @click="$emit('close')" 
          class="absolute top-2 right-2 text-black hover:text-red-600 transition duration-300">
          &times;
        </button>
  
        <UForm
          :validate="validate"
          :state="state"
          class="p-6 bg-white dark:bg-gray-800 rounded-lg shadow-lg max-w-lg w-full"
          @submit="onSubmit"
          @error="onError"
        >
          <h2 class="text-2xl font-semibold text-center mb-4 text-black dark:text-white">Add a Bonus Question</h2>
  
          <UFormGroup label="Question" name="question">
            <UInput v-model="state.question" class="border rounded-md p-2" placeholder="Enter your question here"/>
          </UFormGroup>
  
          <div class="flex justify-between mt-6">
            <UButton @click="$emit('close')" class="bg-gray-300 hover:bg-gray-400 text-black font-bold py-2 px-4 rounded-md transition duration-300">
              Close
            </UButton>
            <UButton type="submit" class="add-button text-white font-bold py-2 px-4 rounded-md transition duration-300">
              Add
            </UButton>
          </div>
        </UForm>
      </div>
    </div>
  </template>
  
  <script setup lang="ts">
  import { reactive } from 'vue';
  import type { FormError, FormErrorEvent } from "#ui/types";
  import type { BonusQuestion } from "~/types/bonusQuestion";
  import { usePredictionGameStore } from '@/stores/stores';
  
  const { addPredictionGameBonusQuestion } = usePredictionGameStore();
  const emit = defineEmits(['close', 'refresh']);
  
  const props = defineProps<{ predictionGameId: number }>();
  
  const state = reactive<BonusQuestion>({
    id: 0,
    predictionGameId: 0,
    question: ""
  });
  
  const validate = (state: any): FormError[] => {
    const errors = [];
    if (!state.question.trim()) errors.push({ path: "question", message: "Question cannot be empty" });
    return errors;
  };
  
  async function onSubmit() {
    const payload = {
      id: state.id,
      question: state.question,
      predictionGameId: props.predictionGameId,
    };
  
    try {
      await addPredictionGameBonusQuestion(payload);
      emit('refresh');
      emit('close');
    } catch (error) {
      console.error("Error adding bonus question:", error);
    }

    window.location.reload();
  }
  
  function onError(event: FormErrorEvent) {
    const element = document.getElementById(event.errors[0].id);
    element?.focus();
    element?.scrollIntoView({ behavior: "smooth", block: "center" });
  }
  </script>
  
  <style>
  .add-button {
    background-color: #5bb17c;
  }
  .add-button:hover {
    background-color: #26547C;
  }
  
  /* Adjust label colors for dark mode */
  div :deep(label) {
    color: black !important;
  }
  @media (prefers-color-scheme: dark) {
    div :deep(label) {
      color: #ffffff !important;
    }
  }
  </style>
  